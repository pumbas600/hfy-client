using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface IHistoryRepository
  {
    public Task<HistoryEntry> AddHistoryEntryAsync(HistoryEntry historyEntry);
  }
}
