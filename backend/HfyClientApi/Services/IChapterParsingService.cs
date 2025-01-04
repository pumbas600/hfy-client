using HfyClientApi.Models;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public interface IChapterParsingService
  {
    DateTime GetEditedAtUtc(SelfPost post);

    Task<string?> GetShareLinkLocationAsync(string shortLink);

    Task<Chapter> ChapterFromPostAsync(SelfPost post);
  }
}
