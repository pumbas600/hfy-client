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
        RedditPostLink = $"{Config.RedditUrl}/r/{chapter.Subreddit}/comments/{chapter.Id}",
        RedditAuthorLink = $"{Config.RedditUrl}/user/{chapter.Author}",
        CreatedAtUtc = chapter.CreatedAtUtc,
        EditedAtUtc = chapter.EditedAtUtc,
        ProcessedAtUtc = chapter.ProcessedAtUtc,
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
        CreatedAtUtc = chapter.CreatedAtUtc,
        EditedAtUtc = chapter.EditedAtUtc,
        ProcessedAtUtc = chapter.ProcessedAtUtc
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
  }
}
