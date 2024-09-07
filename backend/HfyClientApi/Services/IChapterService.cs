using HfyClientApi.Dtos;
using HfyClientApi.Utils;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public interface IChapterService
  {
    Task<Result<FullChapterDto>> GetChapterByIdAsync(string id);

    Task<Result<FullChapterDto>> ProcessChapterByIdAsync(string id);

    Task<Result<FullChapterDto>> ProcessChapterByPostAsync(SelfPost post);

    Task ProcessChaptersByPostAsync(IEnumerable<SelfPost> posts);

    Task<ChapterPaginationDto> GetPaginatedNewChaptersMetadataAsync(
      string subreddit, int pageSize, ChapterPaginationKey? nextKey);
  }
}
