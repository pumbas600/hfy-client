using HfyClientApi.Utils;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public interface IRedditService
  {
    public Result<SelfPost> GetSelfPostById(string postId);
  }
}
