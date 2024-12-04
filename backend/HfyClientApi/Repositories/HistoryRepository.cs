using HfyClientApi.Data;
using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
        .DistinctBy(entry => entry.Chapter.FirstChapterId)
        .LeftJoin(_context.StoryMetadata,
          entry => entry.Chapter.FirstChapterId,
          story => story.FirstChapterId,
          (entry, story) => new CombinedChapter() { Chapter = entry.Chapter, StoryMetadata = story })
        .ToListAsync();

      return currentlyReadingChapters;
    }
  }
}
