using System.Linq.Expressions;
using HfyClientApi.Data;
using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
using HfyClientApi.Models;
using HfyClientApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Repositories
{
  public class ChapterRepository : IChapterRepository
  {
    private const int MaxUpsertAttempts = 3;
    private readonly AppDbContext _context;
    private readonly ILogger<ChapterRepository> _logger;

    public ChapterRepository(AppDbContext context, ILogger<ChapterRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task<Result<Chapter>> UpsertChapterAsync(Chapter chapter)
    {
      for (int attempt = 1; attempt <= MaxUpsertAttempts; attempt++)
      {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
          var detachedChapter = await GetDetachedChapterByIdAsync(chapter.Id);
          if (detachedChapter != null)
          {
            await UpdateChapterAsync(chapter);
          }
          else
          {
            await _context.Chapters.AddAsync(chapter);
            await _context.SaveChangesAsync();
          }

          await transaction.CommitAsync();
          return chapter;
        }
        catch (OperationCanceledException ex)
        {
          await transaction.RollbackAsync();
          _logger.LogError(
            ex, "Upsert chapter id={} transaction cancelled, attempt={}/{}",
            chapter.Id, attempt, MaxUpsertAttempts
          );
        }
      }

      return Errors.ChapterUpsertFailed(chapter.Id);
    }

    public async Task<Result<CombinedChapter>> GetChapterByIdAsync(string id)
    {
      var combinedChapter = await _context.Chapters.Join(
        _context.StoryMetadata,
        chapter => chapter.FirstChapterId,
        story => story.FirstChapterId,
        (chapter, story) => new CombinedChapter() { Chapter = chapter, StoryMetadata = story }
      ).FirstOrDefaultAsync(c => c.Chapter.Id == id);

      if (combinedChapter == null)
      {
        return Errors.ChapterNotFound(id);
      }

      return combinedChapter;
    }

    public async Task<Chapter> UpdateChapterAsync(Chapter chapter, bool onlyLinks = false, bool track = false)
    {
      if (onlyLinks)
      {
        await _context.Chapters
          .Where(c => c.Id == chapter.Id)
          .ExecuteUpdateAsync(builder =>
            builder
              .SetProperty(c => c.PreviousChapterId, chapter.PreviousChapterId)
              .SetProperty(c => c.NextChapterId, chapter.NextChapterId)
              .SetProperty(c => c.FirstChapterId, chapter.FirstChapterId)
          );
        await _context.SaveChangesAsync();
      }
      else
      {
        chapter.SyncedAtUtc = DateTime.UtcNow;
        var entity = _context.Chapters.Update(chapter);
        await _context.SaveChangesAsync();

        if (!track)
        {
          entity.State = EntityState.Detached;
        }
      }

      return chapter;
    }

    private async Task<Chapter?> GetDetachedChapterByIdAsync(string id)
    {
      return await _context.Chapters.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<CombinedChapter>> GetPaginatedNewChaptersMetadataAsync(
      string subreddit, int pageSize, ChapterPaginationKey? nextKey)
    {
      Expression<Func<CombinedChapter, bool>> predicate = nextKey == null
        ? c => c.Chapter.Subreddit == subreddit
        : c => c.Chapter.Subreddit == subreddit && c.Chapter.CreatedAtUtc < nextKey.LastCreatedAtUtc
          || (c.Chapter.CreatedAtUtc == nextKey.LastCreatedAtUtc && c.Chapter.Id.CompareTo(nextKey.LastPostId) > 0);

      return await GetPaginatedChaptersAsync(predicate, pageSize);
    }

    public async Task<IEnumerable<CombinedChapter>> GetPaginatedChaptersMetadataByTitleAsync(
      string subreddit, string title, int pageSize, ChapterPaginationKey? nextKey)
    {
      Expression<Func<CombinedChapter, bool>> predicate = nextKey == null
        ? c => c.Chapter.Subreddit == subreddit && c.Chapter.Title.Contains(title)
        : c => c.Chapter.Subreddit == subreddit && c.Chapter.Title.Contains(title)
          && c.Chapter.CreatedAtUtc < nextKey.LastCreatedAtUtc
          || (c.Chapter.CreatedAtUtc == nextKey.LastCreatedAtUtc && c.Chapter.Id.CompareTo(nextKey.LastPostId) > 0);

      return await GetPaginatedChaptersAsync(predicate, pageSize);
    }

    internal async Task<IEnumerable<CombinedChapter>> GetPaginatedChaptersAsync(
      Expression<Func<CombinedChapter, bool>> predicate, int pageSize)
    {
      return await _context.Chapters
        // This is essentially doing a LEFT JOIN
        .GroupJoin(
          _context.StoryMetadata,
          chapter => chapter.FirstChapterId,
          story => story.FirstChapterId,
          (chapter, story) => new { Chapter = chapter, Story = story }
        )
        .SelectMany(
          x => x.Story.DefaultIfEmpty(),
          (chapter, story) => new CombinedChapter()
          {
            Chapter = chapter.Chapter,
            StoryMetadata = story
          }
        )
        .OrderByDescending(c => c.Chapter.CreatedAtUtc)
        .ThenBy(c => c.Chapter.Id)
        .Where(predicate)
        .Take(pageSize)
        .ToListAsync();
    }

    public async Task<(Chapter?, Chapter?)> GetLinkedChaptersByChapterAsync(Chapter chapter)
    {
      var linkedChapters = await _context.Chapters
        .Select(c => new Chapter()
        {
          Id = c.Id,
          PreviousChapterId = c.PreviousChapterId,
          NextChapterId = c.NextChapterId,
          FirstChapterId = c.FirstChapterId,
        })
        .Where(c => c.Id == chapter.PreviousChapterId || c.Id == chapter.NextChapterId)
        .ToListAsync();
      var linkedChapterMap = linkedChapters.ToDictionary(c => c.Id);

      return (
        linkedChapterMap.GetValueOrDefault(chapter.PreviousChapterId ?? ""),
        linkedChapterMap.GetValueOrDefault(chapter.NextChapterId ?? "")
      );
    }

    public async Task<Chapter?> GetChapterByNextLinkIdAsync(string nextLinkId)
    {
      return await _context.Chapters
        .Select(c => new Chapter()
        {
          Id = c.Id,
          PreviousChapterId = c.PreviousChapterId,
          NextChapterId = c.NextChapterId,
          FirstChapterId = c.FirstChapterId,
        })
        .Where(c => c.NextChapterId == nextLinkId)
        .FirstOrDefaultAsync();
    }

    public async Task<Chapter?> GetChapterByPreviousLinkIdAsync(string previousLinkId)
    {
      return await _context.Chapters
        .Select(c => new Chapter()
        {
          Id = c.Id,
          PreviousChapterId = c.PreviousChapterId,
          NextChapterId = c.NextChapterId
        })
        .Where(c => c.PreviousChapterId == previousLinkId)
        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Chapter>> GetChaptersByIdsAsync(IEnumerable<string> ids)
    {
      return await _context.Chapters
        .AsNoTracking()
        .Where(c => ids.Contains(c.Id))
        .ToListAsync();
    }

    public async Task<Chapter> CreateChapterAsync(Chapter chapter, bool track = false)
    {
      var entity = await _context.Chapters.AddAsync(chapter);
      await _context.SaveChangesAsync();

      if (!track)
      {
        entity.State = EntityState.Detached;
      }

      return chapter;
    }
  }
}
