using HfyClientApi.Models;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public interface IChapterParsingService
  {
    public class ParsedChapter : Chapter
    {
      public string? FirstChapterId { get; set; }

      public bool IsFirstChapter => FirstChapterId == null || FirstChapterId == Id;
    }

    public ParsedChapter ChapterFromPost(SelfPost post);
  }
}
