using HfyClientApi.Dtos;
using HfyClientApi.Models;
using HfyClientApi.Utils;

namespace HfyClientApi.Repositories
{
  public interface IChapterRepository
  {

    Task<Result<Chapter>> GetChapterByIdAsync(string id);

    Task<(Chapter?, Chapter?)> GetLinkedChaptersByChapterAsync(Chapter chapter);

    Task<Chapter?> GetChapterByNextLinkIdAsync(string nextLinkId);

    Task<Chapter?> GetChapterByPreviousLinkIdAsync(string previousLinkId);

    Task<Result<Chapter>> UpsertChapterAsync(Chapter chapter);

    Task<Chapter> UpdateChapterAsync(Chapter chapter, bool onlyLinks = false);

    Task<IEnumerable<Chapter>> GetPaginatedNewChaptersMetadataAsync(
      string subreddit, int pageSize, ChapterPaginationKey? nextKey);
  }

}
