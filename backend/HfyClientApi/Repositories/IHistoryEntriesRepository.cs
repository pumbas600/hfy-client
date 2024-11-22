using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface IHistoryEntriesRepository
  {
    public Task<HistoryEntry> AddHistoryEntryAsync(HistoryEntry historyEntry);
  }
}
