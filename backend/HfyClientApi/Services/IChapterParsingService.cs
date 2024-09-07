using HfyClientApi.Models;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public interface IChapterParsingService
  {
    DateTime GetEditedAtUtc(SelfPost post);

    Task<(Chapter, StoryMetadata?)> ChapterFromPostAsync(SelfPost post);
  }
}
