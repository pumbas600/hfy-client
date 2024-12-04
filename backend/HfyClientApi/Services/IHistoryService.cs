using HfyClientApi.Dtos;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public interface IHistoryService
  {
    Task<Result<HistoryEntryDto>> AddHistoryEntryAsync(string id, string readerName);
  }
}
