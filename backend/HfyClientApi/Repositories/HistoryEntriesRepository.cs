using HfyClientApi.Data;
using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public class HistoryEntriesRepository : IHistoryEntriesRepository
  {
    private readonly AppDbContext _context;

    public HistoryEntriesRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<HistoryEntry> AddHistoryEntryAsync(HistoryEntry historyEntry)
    {
      await _context.AddAsync(historyEntry);
      return historyEntry;
    }
  }
}
