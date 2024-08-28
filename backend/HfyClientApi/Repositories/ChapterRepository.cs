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

    public async Task<Chapter?> GetChapterByIdAsync(string id)
    {
      return await _context.Chapters.FindAsync(id);
    }
  }
}
