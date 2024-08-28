using HfyClientApi.Models;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public interface IChapterParsingService
  {
    public Chapter ChapterFromPost(SelfPost post);
  }
}
