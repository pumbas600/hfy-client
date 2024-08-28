using HfyClientApi.Data;
using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public class ChapterRepository : IChapterRepository
  {
    private readonly AppDbContext _context;

    public ChapterRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<Chapter> CreateChapterAsync(Chapter chapter)
    {
      await _context.Chapters.AddAsync(chapter);
      await _context.SaveChangesAsync();
      return chapter;
    }

    public async Task<Chapter> CreateFirstChapter(Story story, Chapter firstChapter)
    {
      firstChapter.Story = story;

      await _context.Chapters.AddAsync(firstChapter);
      await _context.SaveChangesAsync();

      story.FirstChapter = firstChapter;
      await _context.SaveChangesAsync();

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
  }
}
