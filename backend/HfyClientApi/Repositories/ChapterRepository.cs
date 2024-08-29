using HfyClientApi.Data;
using HfyClientApi.Exceptions;
using HfyClientApi.Models;
using HfyClientApi.Utils;

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
      for (int attempt = 0; attempt < MaxUpsertAttempts; attempt++)
      {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
          var existingChapterResult = await GetChapterByIdAsync(chapter.Id);
          if (existingChapterResult.IsSuccess)
          {
            chapter.Story = existingChapterResult.Data.Story;
            await UpdateChapterAsync(chapter);
          }
          else
          {
            await _context.Chapters.AddAsync(chapter);
            await _context.SaveChangesAsync();
            await _context.Entry(chapter).Reference(c => c.Story).LoadAsync();
          }

          transaction.Commit();

          return chapter;
        }
        catch (Exception)
        {
          await transaction.RollbackAsync();
          _logger.LogError(
            "Failed to upsert chapter id={}, attempt={}/{}",
            chapter.Id, attempt, MaxUpsertAttempts
          );
        }
      }

      return Errors.ChapterUpsertFailed(chapter.Id);
    }

    public async Task<Result<Chapter>> UpsertFirstChapter(Story story, Chapter firstChapter)
    {
      for (int attempt = 0; attempt < MaxUpsertAttempts; attempt++)
      {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
          var existingChapterResult = await GetChapterByIdAsync(firstChapter.Id);
          if (existingChapterResult.IsSuccess)
          {
            firstChapter.Story = existingChapterResult.Data.Story;
            await UpdateChapterAsync(firstChapter);
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

          transaction.Commit();
          return firstChapter;
        }
        catch (Exception)
        {
          await transaction.RollbackAsync();
          _logger.LogError(
            "Failed to upsert first chapter id={}, attempt={}/{}",
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
  }
}
