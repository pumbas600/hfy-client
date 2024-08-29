using HfyClientApi.Models;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public interface IChapterParsingService
  {
    public class ParsedChapter : Chapter
    {
      private string? _firstChapterId;

      public string? FirstChapterId
      {
        get
        {
          if (_firstChapterId != null)
          {
            return _firstChapterId;
          }
          if (PreviousChapterId == null)
          {
            return Id;
          }

          return null;
        }
        set
        {
          _firstChapterId = value;
        }
      }
    }

    public ParsedChapter ChapterFromPost(SelfPost post);
  }
}
