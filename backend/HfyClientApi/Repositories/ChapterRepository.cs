using HfyClientApi.Data;
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
            chapter.StoryId = detachedChapter.StoryId;
            await UpdateChapterAsync(chapter);
          }
          else
          {
            await _context.Chapters.AddAsync(chapter);
            await _context.SaveChangesAsync();
          }

          await _context.Entry(chapter).Reference(c => c.Story).LoadAsync();
          await transaction.CommitAsync();

          return chapter;
        }
        catch (OperationCanceledException ex)
        {
          await transaction.RollbackAsync();
          _logger.LogError(
            ex, "Failed to upsert chapter id={}, attempt={}/{}",
            chapter.Id, attempt, MaxUpsertAttempts
          );
        }
      }

      return Errors.ChapterUpsertFailed(chapter.Id);
    }

    public async Task<Result<Chapter>> UpsertFirstChapterAsync(Story story, Chapter firstChapter)
    {
      for (int attempt = 1; attempt <= MaxUpsertAttempts; attempt++)
      {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
          var detachedChapter = await GetDetachedChapterByIdAsync(firstChapter.Id);
          if (detachedChapter != null)
          {
            firstChapter.StoryId = detachedChapter.StoryId;
            await UpdateChapterAsync(firstChapter);
            await _context.Entry(firstChapter).Reference(c => c.Story).LoadAsync();
            // Note: Theoretically the story entity should never need to be updated. -P
          }
          else
          {
            firstChapter.Story = story;
            await _context.Chapters.AddAsync(firstChapter);
            await _context.SaveChangesAsync();

            story.FirstChapter = firstChapter;
            await _context.SaveChangesAsync();
          }

          await transaction.CommitAsync();
          return firstChapter;
        }
        catch (OperationCanceledException ex)
        {
          await transaction.RollbackAsync();
          _logger.LogError(
            ex, "Failed to upsert first chapter id={}, attempt={}/{}",
            firstChapter.Id, attempt, MaxUpsertAttempts
          );
        }
      }

      return Errors.ChapterUpsertFailed(firstChapter.Id);
    }

    public async Task<Result<Chapter>> GetChapterByIdAsync(string id)
    {
      var chapter = await _context.Chapters.FindAsync(id);
      if (chapter == null)
      {
        return Errors.ChapterNotFound(id);
      }


      // By loading the Story like this, rather than using .Include(), we avoid a round-trip to
      // the database if the entity is already local storage with Find().
      await _context.Entry(chapter).Reference(c => c.Story).LoadAsync();
      return chapter;
    }

    public async Task<Chapter> UpdateChapterAsync(Chapter chapter)
    {
      _context.Chapters.Update(chapter);
      await _context.SaveChangesAsync();
      return chapter;
    }

    private async Task<Chapter?> GetDetachedChapterByIdAsync(string id)
    {
      return await _context.Chapters.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }
  }
}
