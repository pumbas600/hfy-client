using HfyClientApi.Dtos;
using HfyClientApi.Models;
using HfyClientApi.Utils;

namespace HfyClientApi.Repositories
{

  public interface IChapterRepository
  {
    Task<IEnumerable<Chapter>> GetChaptersByIdsAsync(IEnumerable<string> ids);

    Task<Result<Chapter>> GetChapterByIdAsync(string id);

    Task<(Chapter?, Chapter?)> GetLinkedChaptersByChapterAsync(Chapter chapter);

    Task<Chapter?> GetChapterByNextLinkIdAsync(string nextLinkId);

    Task<Chapter?> GetChapterByPreviousLinkIdAsync(string previousLinkId);

    Task<Result<Chapter>> UpsertChapterAsync(Chapter chapter);

    Task<Chapter> CreateChapterAsync(Chapter chapter, bool track = false);

    Task<Chapter> UpdateChapterAsync(Chapter chapter, bool onlyLinks = false, bool track = false);

    Task<IEnumerable<Chapter>> GetPaginatedChaptersMetadataByTitleAsync(
      string subreddit, string title, int pageSize, ChapterPaginationKey? nextKey);

    Task<IEnumerable<Chapter>> GetPaginatedNewChaptersMetadataAsync(
      string subreddit, int pageSize, ChapterPaginationKey? nextKey);
  }

}
