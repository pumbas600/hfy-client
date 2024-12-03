using HfyClientApi.Dtos;
using HfyClientApi.Models;
using HfyClientApi.Repositories;

namespace HfyClientApi.Services
{
  public interface IMapper
  {
    FullChapterDto ToFullChapterDto(CombinedChapter combinedChapter);

    ChapterMetadataDto ToChapterMetadataDto(CombinedChapter combinedChapter);

    ChapterPaginationDto ToPaginatedChapterMetadataDto(int pageSize, IEnumerable<CombinedChapter> combinedChapters);

    SubredditDto ToSubredditDto(Subreddit subreddit);

    UserDto ToUserDto(User user);

    HistoryEntryDto ToHistoryEntryDto(HistoryEntry historyEntry);
  }
}
