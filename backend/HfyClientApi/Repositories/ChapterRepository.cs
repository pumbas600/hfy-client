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

    public async Task<Result<Chapter>> GetChapterByIdAsync(string id)
    {
      var chapter = await _context.Chapters.FindAsync(id);
      if (chapter == null)
      {
        return Errors.ChapterNotFound(id);
      }

      return chapter;
    }

    public async Task<Chapter> UpdateChapterAsync(Chapter chapter, bool onlyLinks = false)
    {
      if (onlyLinks)
      {
        await _context.Chapters
          .Where(c => c.Id == chapter.Id)
          .ExecuteUpdateAsync(builder =>
            builder.SetProperty(c => c.PreviousChapterId, chapter.PreviousChapterId)
              .SetProperty(c => c.NextChapterId, chapter.NextChapterId)
          );
      }
      else
      {
        chapter.ProcessedAtUtc = DateTime.UtcNow;
        _context.Chapters.Update(chapter);
      }
      await _context.SaveChangesAsync();
      return chapter;
    }

    private async Task<Chapter?> GetDetachedChapterByIdAsync(string id)
    {
      return await _context.Chapters.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Chapter>> GetPaginatedNewChaptersMetadataAsync(
      string subreddit, int pageSize, ChapterPaginationKey? nextKey)
    {
      Expression<Func<Chapter, bool>> predicate = nextKey == null
        ? c => c.Subreddit == subreddit
        : c => c.Subreddit == subreddit && c.CreatedAtUtc < nextKey.LastCreatedAtUtc
          || (c.CreatedAtUtc == nextKey.LastCreatedAtUtc && c.Id.CompareTo(nextKey.LastPostId) > 0);

      return await _context.Chapters
        .Select(c => new Chapter()
        {
          Id = c.Id,
          Author = c.Author,
          Subreddit = c.Subreddit,
          Title = c.Title,
          IsNsfw = c.IsNsfw,
          Upvotes = c.Upvotes,
          Downvotes = c.Downvotes,
          CreatedAtUtc = c.CreatedAtUtc,
          EditedAtUtc = c.EditedAtUtc,
          ProcessedAtUtc = c.ProcessedAtUtc
        })
        .OrderByDescending(c => c.CreatedAtUtc)
        .ThenBy(c => c.Id)
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
          NextChapterId = c.NextChapterId
        })
        .Where(c => c.Id == chapter.PreviousChapterId || c.Id == chapter.NextChapterId)
        .ToListAsync();
      var linkedChapterMap = linkedChapters.ToDictionary(c => c.Id);

      return (
        linkedChapterMap.GetValueOrDefault(chapter.PreviousChapterId ?? ""),
        linkedChapterMap.GetValueOrDefault(chapter.NextChapterId ?? "")
      );
    }
  }
}
