using HfyClientApi.Models;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public interface IChapterParsingService
  {
    Task<(Chapter, StoryMetadata?)> ChapterFromPostAsync(SelfPost post);
  }
}
