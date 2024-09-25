using HfyClientApi.Utils;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public interface IRedditService
  {
    Result<SelfPost> GetSelfPostById(string postId);

    IEnumerable<SelfPost> GetNewSelfPosts(string subreddit, int limit = 50);

    Task RevokeAccessTokenAsync(string accessToken);
  }
}
