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
      return await _context.Chapters.FindAsync(id);
    }
  }
}
