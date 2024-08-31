using HfyClientApi.Exceptions;
using HfyClientApi.Utils;
using Reddit;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public class RedditService : IRedditService
  {
    private readonly RedditClient _redditClient;

    public RedditService(RedditClient redditClient)
    {
      _redditClient = redditClient;
    }

    public Result<SelfPost> GetSelfPostById(string postId)
    {
      var postFullname = $"t3_{postId}";
      var selfPost = _redditClient.SelfPost(postFullname).About();
      if (selfPost == null)
      {
        return Errors.PostNotFound(postId);
      }

      return selfPost;
    }
  }
}
