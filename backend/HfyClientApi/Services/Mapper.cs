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
  }
}
