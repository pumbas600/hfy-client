using HfyClientApi.Exceptions;
using HfyClientApi.Utils;
using Reddit;
using Reddit.Controllers;
using Reddit.Exceptions;

namespace HfyClientApi.Services
{
  public class RedditService : IRedditService
  {
    private readonly RedditClient _reddit;

    public RedditService(RedditClient redditClient)
    {
      _reddit = redditClient;
    }

    public Result<SelfPost> GetSelfPostById(string postId)
    {
      try
      {
        var postFullname = $"t3_{postId}";
        var posts = _reddit.GetPosts([postFullname]);
        if (posts.Count == 0)
        {
          return Errors.PostNotFound(postId);
        }

        var post = posts[0];
        if (post is not SelfPost selfPost)
        {
          return Errors.PostNotSelfPost(postId);
        }

        return selfPost;
      }
      catch (RedditNotFoundException)
      {
        return Errors.PostNotFound(postId);
      }
    }
  }
}
