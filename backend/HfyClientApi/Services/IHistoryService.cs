using HfyClientApi.Dtos;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public interface IHistoryService
  {
    Task<Result<IEnumerable<ChapterMetadataDto>>> GetCurrentlyReadingChaptersAsync(string userName);

    Task<Result<HistoryEntryDto>> AddHistoryEntryAsync(string id, string readerName);
  }
}
