using HfyClientApi.Data;
using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public class ChapterRepository : IChapterRepository
  {
    private readonly AppDbContext _context;
    private readonly ILogger<ChapterRepository> _logger;

    public ChapterRepository(AppDbContext context, ILogger<ChapterRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task<Chapter> UpsertChapterAsync(Chapter chapter)
    {
      using var transaction = await _context.Database.BeginTransactionAsync();

      try
      {
        var existingChapter = await GetChapterByIdAsync(chapter.Id);
        if (existingChapter != null)
        {
          chapter.Story = existingChapter.Story;
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
        _logger.LogError("Failed to upsert chapter (id={})", chapter.Id);
        await transaction.RollbackAsync();
      }

      // TODO: The story might not be populated. -P
      return chapter;
    }

    public async Task<Chapter> UpsertFirstChapter(Story story, Chapter firstChapter)
    {
      firstChapter.Story = story;

      using var transaction = await _context.Database.BeginTransactionAsync();

      try
      {
        var existingChapter = await GetChapterByIdAsync(firstChapter.Id);
        if (existingChapter != null)
        {
          firstChapter.Story = existingChapter.Story;
          await UpdateChapterAsync(firstChapter);
          // Note: Theoretically the story entity should never need to be updated. -P
        }
        else
        {
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
        _logger.LogError("Failed to upsert first chapter (id={})", firstChapter.Id);
        await transaction.RollbackAsync();
      }

      // TODO: The story might not be populated. -P
      return firstChapter;
    }

    public async Task<Chapter?> GetChapterByIdAsync(string id)
    {
      var chapter = await _context.Chapters.FindAsync(id);
      if (chapter != null)
      {
        // By loading the Story like this, rather than using .Include(), we avoid a round-trip to
        // the database if the entity is already local storage with Find().
        await _context.Entry(chapter).Reference(c => c.Story).LoadAsync();
      }

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
