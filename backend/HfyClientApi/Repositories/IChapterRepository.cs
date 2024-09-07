using HfyClientApi.Dtos;
using HfyClientApi.Models;
using HfyClientApi.Utils;

namespace HfyClientApi.Repositories
{
  public class CombinedChapter
  {
    public required Chapter Chapter { get; set; }
    public required StoryMetadata? StoryMetadata { get; set; }
  }

  public interface IChapterRepository
  {
    Task<IEnumerable<Chapter>> GetChaptersByIdsAsync(IEnumerable<string> ids);

    Task<Result<CombinedChapter>> GetChapterByIdAsync(string id);

    Task<(Chapter?, Chapter?)> GetLinkedChaptersByChapterAsync(Chapter chapter);

    Task<Chapter?> GetChapterByNextLinkIdAsync(string nextLinkId);

    Task<Chapter?> GetChapterByPreviousLinkIdAsync(string previousLinkId);

    Task<Result<Chapter>> UpsertChapterAsync(Chapter chapter);

    Task<Chapter> CreateChapterAsync(Chapter chapter, bool track = false);

    Task<Chapter> UpdateChapterAsync(Chapter chapter, bool onlyLinks = false, bool track = false);

    Task<IEnumerable<CombinedChapter>> GetPaginatedNewChaptersMetadataAsync(
      string subreddit, int pageSize, ChapterPaginationKey? nextKey);
  }

}
