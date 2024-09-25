using HfyClientApi.Configuration;
using HfyClientApi.Exceptions;
using HfyClientApi.Utils;
using Reddit;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public class RedditService : IRedditService
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RedditClient _redditClient;
    private readonly ILogger<RedditService> _logger;

    public RedditService(
      IHttpClientFactory httpClientFactory, RedditClient redditClient,
      ILogger<RedditService> logger)
    {
      _httpClientFactory = httpClientFactory;
      _redditClient = redditClient;
      _logger = logger;
    }

    public IEnumerable<SelfPost> GetNewSelfPosts(string subreddit, int limit = 50)
    {
      return _redditClient.Subreddit(subreddit).Posts
        .GetNew(limit: limit)
        .Where(p => p is SelfPost)
        .Cast<SelfPost>();
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

    public async Task RevokeAccessTokenAsync(string accessToken)
    {
      using HttpClient client = _httpClientFactory.CreateClient();

      try
      {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
          {
            { "token", accessToken },
            { "token_type_hint", "access_token" }
          }
        );

        var response = await client.PostAsync(Config.RedditUrl + "/api/v1/revoke_token", content);
        response.EnsureSuccessStatusCode();
      }
      catch (HttpRequestException e)
      {
        _logger.LogError(e, "Failed to revoke Reddit access token.");
      }
    }
  }
}
