using HfyClientApi.Utils;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public interface IRedditService
  {
    Result<SelfPost> GetSelfPostById(string postId);
  }
}
