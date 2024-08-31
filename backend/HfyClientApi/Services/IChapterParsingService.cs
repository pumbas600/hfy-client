using HfyClientApi.Models;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public interface IChapterParsingService
  {
    Chapter ChapterFromPost(SelfPost post);
  }
}
