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
        ChapterId = chapter.Id,
        Title = chapter.Title,
        TextHtml = chapter.TextHtml,
        IsNsfw = chapter.IsNsfw,
        Created = chapter.Created,
        Edited = chapter.Edited,
        NextChapterId = chapter.NextChapterId,
        PreviousChapterId = chapter.PreviousChapterId,
        StoryId = chapter.Story.Id,
        Author = chapter.Story.Author,
        Subreddit = chapter.Story.Subreddit,
        FirstChapterId = chapter.Story.FirstChapterId // TODO: Make this always non-null...
      };
    }
  }
}
