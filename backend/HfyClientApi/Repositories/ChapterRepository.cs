using System.Linq.Expressions;
using System.Text.RegularExpressions;
using HfyClientApi.Data;
using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
using HfyClientApi.Models;
using HfyClientApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Repositories
{
  public partial class ChapterRepository : IChapterRepository
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
            await CreateChapterAsync(chapter);
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
      var chapter = await _context.Chapters.FirstOrDefaultAsync(c => c.Id == id);

      if (chapter == null)
      {
        return Errors.ChapterNotFound(id);
      }

      return chapter;
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
        chapter.SearchableTitle = GetSearchableTitle(chapter.Title);
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

    public async Task<IEnumerable<Chapter>> GetPaginatedNewChaptersMetadataAsync(
      string subreddit, int pageSize, ChapterPaginationKey? nextKey)
    {
      var uppercaseSubreddit = subreddit.ToUpper();

      Expression<Func<Chapter, bool>> predicate = nextKey == null
        ? c => c.Subreddit.ToUpper() == uppercaseSubreddit
        : c => c.Subreddit.ToUpper() == uppercaseSubreddit
          && c.CreatedAtUtc < nextKey.LastCreatedAtUtc
          || (c.CreatedAtUtc == nextKey.LastCreatedAtUtc && c.Id.CompareTo(nextKey.LastPostId) > 0);

      return await GetPaginatedChaptersAsync(predicate, pageSize);
    }

    public async Task<IEnumerable<Chapter>> GetPaginatedChaptersMetadataByTitleAsync(
      string subreddit, string title, int pageSize, ChapterPaginationKey? nextKey)
    {
      var uppercaseSubreddit = subreddit.ToUpper();
      var searchableTitle = GetSearchableTitle(title);

      Expression<Func<Chapter, bool>> predicate = nextKey == null
        ? c => c.Subreddit.ToUpper() == uppercaseSubreddit
          && c.SearchableTitle.Contains(searchableTitle)
        : c => c.Subreddit.ToUpper() == uppercaseSubreddit
          && c.SearchableTitle.Contains(searchableTitle)
          && c.CreatedAtUtc < nextKey.LastCreatedAtUtc
          || (c.CreatedAtUtc == nextKey.LastCreatedAtUtc && c.Id.CompareTo(nextKey.LastPostId) > 0);

      return await GetPaginatedChaptersAsync(predicate, pageSize);
    }

    internal async Task<IEnumerable<Chapter>> GetPaginatedChaptersAsync(
      Expression<Func<Chapter, bool>> predicate, int pageSize,
      Expression<Func<Chapter, object>>? orderBy = null)
    {
      var query = _context.Chapters;

      var orderedQuery = orderBy != null
        ? query.OrderBy(orderBy).ThenByDescending(c => c.CreatedAtUtc)
        : query.OrderByDescending(c => c.CreatedAtUtc);

      return await orderedQuery
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
      chapter.SearchableTitle = GetSearchableTitle(chapter.Title);
      var entity = await _context.Chapters.AddAsync(chapter);
      await _context.SaveChangesAsync();

      if (!track)
      {
        entity.State = EntityState.Detached;
      }

      return chapter;
    }

    internal static string GetSearchableTitle(string title)
    {
      var withEmptySpecialCharactersRemoved = EmptySpecialCharacterRegex().Replace(title, "");
      return SpecialCharacterRegex().Replace(withEmptySpecialCharactersRemoved, " ").ToLower();
    }

    [GeneratedRegex("['’]+")]
    private static partial Regex EmptySpecialCharacterRegex();


    [GeneratedRegex(@"\W+")]
    private static partial Regex SpecialCharacterRegex();
  }
}
