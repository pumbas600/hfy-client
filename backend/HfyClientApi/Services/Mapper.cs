using HfyClientApi.Configuration;
using HfyClientApi.Dtos;
using HfyClientApi.Models;
using HfyClientApi.Repositories;

namespace HfyClientApi.Services
{
  public class Mapper : IMapper
  {
    public FullChapterDto ToFullChapterDto(CombinedChapter combinedChapter)
    {
      var chapter = combinedChapter.Chapter;

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
        CoverArtUrl = combinedChapter.StoryMetadata?.CoverArtUrl,
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

    public ChapterMetadataDto ToChapterMetadataDto(CombinedChapter combinedChapter)
    {
      var chapter = combinedChapter.Chapter;

      return new ChapterMetadataDto
      {
        Id = chapter.Id,
        Author = chapter.Author,
        Subreddit = chapter.Subreddit,
        Title = chapter.Title,
        IsNsfw = chapter.IsNsfw,
        Upvotes = chapter.Upvotes,
        Downvotes = chapter.Downvotes,
        CoverArtUrl = combinedChapter.StoryMetadata?.CoverArtUrl,
        RedditAuthorLink = $"{Config.RedditUrl}/user/{chapter.Author}",
        CreatedAtUtc = chapter.CreatedAtUtc,
        EditedAtUtc = chapter.EditedAtUtc,
        SyncedAtUtc = chapter.SyncedAtUtc
      };
    }

    public ChapterPaginationDto ToPaginatedChapterMetadataDto(
      int pageSize, IEnumerable<CombinedChapter> combinedChapters)
    {
      var lastChapter = combinedChapters.LastOrDefault();
      var nextKey = lastChapter == null ? null : new ChapterPaginationKey()
      {
        LastCreatedAtUtc = lastChapter.Chapter.CreatedAtUtc,
        LastPostId = lastChapter.Chapter.Id
      };

      return new ChapterPaginationDto()
      {
        NextKey = nextKey,
        PageSize = pageSize,
        Data = combinedChapters.Select(ToChapterMetadataDto)
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
        IconBackgroundColor = subreddit.IconBackgroundColor
      };
    }
  }
}
