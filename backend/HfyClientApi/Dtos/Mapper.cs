using HfyClientApi.Models;

namespace HfyClientApi.Dtos
{
  public static class Mapper
  {
    public static FullChapterDto ToFullChapterDto(Story story, Chapter chapter)
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
