using HfyClientApi.Configuration;
using HfyClientApi.Dtos;
using HfyClientApi.Models;

namespace HfyClientApi.Services
{
  public class Mapper : IMapper
  {
    public FullChapterDto ToFullChapterDto(Chapter chapter)
    {
      return new FullChapterDto
      {
        Id = chapter.Id,
        Author = chapter.Author,
        Subreddit = chapter.Subreddit,
        Title = chapter.Title,
        TextHtml = chapter.TextHtml,
        IsNsfw = chapter.IsNsfw,
        Upvotes = chapter.Upvotes,
        Downvotes = chapter.Downvotes,
        CoverArtUrl = chapter.CoverArtUrl,
        RedditPostLink = $"{Config.RedditUrl}/r/{chapter.Subreddit}/comments/{chapter.Id}",
        RedditAuthorLink = $"{Config.RedditUrl}/user/{chapter.Author}",
        CreatedAtUtc = chapter.CreatedAtUtc,
        EditedAtUtc = chapter.EditedAtUtc,
        SyncedAtUtc = chapter.SyncedAtUtc,
        NextChapterId = chapter.NextChapterId,
        PreviousChapterId = chapter.PreviousChapterId,
        FirstChapterId = chapter.FirstChapterId
      };
    }

    public ChapterMetadataDto ToChapterMetadataDto(Chapter chapter)
    {
      return new ChapterMetadataDto
      {
        Id = chapter.Id,
        Author = chapter.Author,
        Subreddit = chapter.Subreddit,
        Title = chapter.Title,
        IsNsfw = chapter.IsNsfw,
        Upvotes = chapter.Upvotes,
        Downvotes = chapter.Downvotes,
        CoverArtUrl = chapter.CoverArtUrl,
        RedditAuthorLink = $"{Config.RedditUrl}/user/{chapter.Author}",
        CreatedAtUtc = chapter.CreatedAtUtc,
        EditedAtUtc = chapter.EditedAtUtc,
        SyncedAtUtc = chapter.SyncedAtUtc
      };
    }

    public ChapterPaginationDto ToPaginatedChapterMetadataDto(
      int pageSize, IEnumerable<Chapter> chapters)
    {
      var lastChapter = chapters.LastOrDefault();
      var nextKey = lastChapter == null ? null : new ChapterPaginationKey()
      {
        LastCreatedAtUtc = lastChapter.CreatedAtUtc,
        LastPostId = lastChapter.Id
      };

      return new ChapterPaginationDto()
      {
        NextKey = nextKey,
        PageSize = pageSize,
        Data = chapters.Select(ToChapterMetadataDto)
      };
    }

    public SubredditDto ToSubredditDto(Subreddit subreddit)
    {
      return new()
      {
        Name = subreddit.Name,
        Title = subreddit.Title,
        Description = subreddit.Description,
        IconUrl = subreddit.IconUrl,
        IconBackgroundColor = subreddit.IconBackgroundColor,
        RedditLink = $"{Config.RedditUrl}/r/{subreddit.Name}"
      };
    }

    public UserDto ToUserDto(User user)
    {
      return new UserDto
      {
        Name = user.Name,
        IconUrl = user.IconUrl
      };
    }

    public HistoryEntryDto ToHistoryEntryDto(HistoryEntry historyEntry)
    {
      return new HistoryEntryDto
      {
        Id = historyEntry.Id,
        ChapterId = historyEntry.ChapterId,
        ReadAtUtc = historyEntry.ReadAtUtc
      };
    }

    public ReadingHistoryDto ToReadingHistoryDto(HistoryEntry historyEntry)
    {
      return new ReadingHistoryDto
      {
        Id = historyEntry.Id,
        ChapterMetadata = ToChapterMetadataDto(historyEntry.Chapter),
        ReadAtUtc = historyEntry.ReadAtUtc,
        NextChapterId = historyEntry.Chapter.NextChapterId
      };
    }
  }
}
