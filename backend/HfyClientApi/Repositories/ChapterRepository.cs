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
      story.FirstChapter = firstChapter;
      // TODO: Assign story to first chapter?

      await _context.Stories.AddAsync(story);
      await _context.SaveChangesAsync();
      return firstChapter;
    }

    public async Task<Chapter?> GetChapterByIdAsync(string id)
    {
      return await _context.Chapters.FindAsync(id);
    }
  }
}
