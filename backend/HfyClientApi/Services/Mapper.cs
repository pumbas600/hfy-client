using HfyClientApi.Dtos;
using HfyClientApi.Models;

namespace HfyClientApi.Services
{
  public class Mapper : IMapper
  {
    public FullChapterDto ToFullChapterDto(Story story, Chapter chapter)
    {
      return new FullChapterDto
      {
        ChapterId = chapter.Id,
        Title = chapter.Title,
        TextHtml = chapter.TextHtml,
        Created = chapter.Created,
        Edited = chapter.Edited,
        NextChapterId = chapter.NextChapterId,
        PreviousChapterId = chapter.PreviousChapterId,
        StoryId = story.Id,
        Author = story.Author,
        Subreddit = story.Subreddit,
        FirstChapterId = story.FirstChapterId
      };
    }
  }
}
