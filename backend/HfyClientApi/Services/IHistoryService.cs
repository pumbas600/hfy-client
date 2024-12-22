using HfyClientApi.Dtos;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public struct MaybeCreated<T>
  {
    public bool IsCreated { get; set; }
    public T Value { get; set; }
  }

  public interface IHistoryService
  {
    Task<Result<IEnumerable<ChapterMetadataDto>>> GetCurrentlyReadingChaptersAsync(string userName);

    Task<Result<MaybeCreated<HistoryEntryDto>>> AddHistoryEntryAsync(string id, string readerName);
  }
}
