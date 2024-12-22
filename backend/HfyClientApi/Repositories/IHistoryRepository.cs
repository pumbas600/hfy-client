using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface IHistoryRepository
  {
    public Task<IEnumerable<CombinedChapter>> GetCurrentlyReadingChaptersAsync(string userName);

    public Task<HistoryEntry?> GetMostRecentEntryAsync(string userName);

    public Task<HistoryEntry> AddEntryAsync(HistoryEntry historyEntry);
  }
}
