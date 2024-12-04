using HfyClientApi.Data;
using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public class HistoryRepository : IHistoryRepository
  {
    private readonly AppDbContext _context;

    public HistoryRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<HistoryEntry> AddHistoryEntryAsync(HistoryEntry historyEntry)
    {
      await _context.AddAsync(historyEntry);
      await _context.SaveChangesAsync();
      return historyEntry;
    }
  }
}
