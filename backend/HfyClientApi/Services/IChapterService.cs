using HfyClientApi.Dtos;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public interface IChapterService
  {
    Task<Result<FullChapterDto>> GetChapterByIdAsync(string id);

    Task<Result<FullChapterDto>> ProcessChapterByIdAsync(string id);

    Task<ChapterPaginationDto> GetPaginatedNewChaptersMetadataAsync(
      int pageSize, ChapterPaginationKey? nextKey);
  }
}
