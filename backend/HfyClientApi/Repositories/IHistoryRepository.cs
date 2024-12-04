using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface IHistoryRepository
  {
    public Task<IEnumerable<HistoryEntry>> GetCurrentlyReadingChaptersAsync(string userName);

    public Task<HistoryEntry> AddEntryAsync(HistoryEntry historyEntry);
  }
}
