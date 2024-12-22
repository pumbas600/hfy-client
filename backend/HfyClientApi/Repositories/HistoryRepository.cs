using HfyClientApi.Data;
using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Repositories
{
  public class HistoryRepository : IHistoryRepository
  {
    private readonly AppDbContext _context;

    public HistoryRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<HistoryEntry> AddEntryAsync(HistoryEntry historyEntry)
    {
      await _context.AddAsync(historyEntry);
      await _context.SaveChangesAsync();
      return historyEntry;
    }

    public async Task<IEnumerable<CombinedChapter>> GetCurrentlyReadingChaptersAsync(string userName)
    {
      var currentlyReadingChapters = await _context.HistoryEntries
        .Where(entry => entry.UserName == userName)
        .Include(entry => entry.Chapter)
        .OrderBy(entry => entry.ReadAtUtc)
        .GroupJoin(
          _context.StoryMetadata,
          entry => entry.Chapter.FirstChapterId,
          story => story.FirstChapterId,
          (entry, story) => new { entry.Chapter, Story = story }
        )
        .SelectMany(
          x => x.Story.DefaultIfEmpty(),
          (entry, story) => new CombinedChapter()
          {
            Chapter = entry.Chapter,
            StoryMetadata = story
          }
        )
        .GroupBy(chapter => chapter.Chapter.FirstChapterId)
        .Select(group => group.First())
        .ToListAsync();

      return currentlyReadingChapters;
    }

    public async Task<HistoryEntry?> GetMostRecentEntryAsync(string userName)
    {
      return await _context.HistoryEntries
        .Where(entry => entry.UserName == userName)
        .OrderByDescending(entry => entry.ReadAtUtc)
        .FirstOrDefaultAsync();
    }
  }
}
