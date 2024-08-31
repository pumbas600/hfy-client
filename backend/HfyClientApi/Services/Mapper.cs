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
        Upvotes = 1, // TODO: Implement upvotes and downvotes
        Downvotes = 0,
        RedditPostLink = $"https://www.reddit.com/r/{chapter.Subreddit}/comments/{chapter.Id}",
        RedditAuthorLink = $"https://www.reddit.com/user/{chapter.Author}",
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
        Upvotes = 1, // TODO: Implement upvotes and downvotes
        Downvotes = 0,
        CreatedAtUtc = chapter.CreatedAtUtc,
        EditedAtUtc = chapter.EditedAtUtc,
        ProcessedAtUtc = chapter.ProcessedAtUtc
      };
    }

    public ChapterPaginationDto ToPaginatedChapterMetadataDto(
      int pageSize, IEnumerable<Chapter> chapters)
    {
      var lastChapter = chapters.Last();
      var nextKey = new ChapterPaginationKey()
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
