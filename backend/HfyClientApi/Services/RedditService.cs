using System.Text.Json.Serialization;
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
    private readonly IConfiguration _configuration;

    public RedditService(
      IHttpClientFactory httpClientFactory, RedditClient redditClient,
      ILogger<RedditService> logger, IConfiguration configuration)
    {
      _httpClientFactory = httpClientFactory;
      _redditClient = redditClient;
      _logger = logger;
      _configuration = configuration;
    }

    public async Task<Result<string>> GetAccessTokenAsync(string code)
    {
      using HttpClient client = _httpClientFactory.CreateClient();

      var redirectUri = _configuration[Config.Keys.RedditRedirectUri]!;
      var appId = _configuration[Config.Keys.RedditAppId]!;
      var appSecret = _configuration[Config.Keys.RedditAppSecret]!;

      try
      {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
          {
            { "grant_type", "authorization_code" },
            { "code", code },
            { "redirect_uri", redirectUri },
          }
        );

        content.Headers.Add("User-Agent", Config.UserAgent);
        content.Headers.Add("Authorization", $"Basic {appId}:{appSecret}");

        var response = await client.PostAsync(Config.RedditUrl + "/api/v1/access_token", content);
        if (response.Headers.Contains("x-ratelimit-used"))
        {
          _logger.LogCritical("Reddit ratelimit used exchanging code for access token!");
        }

        response.EnsureSuccessStatusCode();

        var accessTokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();
        if (accessTokenResponse?.AccessToken == null)
        {
          return Errors.AuthCodeExchangeError;
        }

        return accessTokenResponse.AccessToken;
      }
      catch (HttpRequestException e)
      {
        _logger.LogError(e, "Failed to exchange code for access token.");
        return Errors.AuthCodeExchangeError;
      }
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

        content.Headers.Add("User-Agent", Config.UserAgent);

        var response = await client.PostAsync(Config.RedditUrl + "/api/v1/revoke_token", content);
        if (response.Headers.Contains("x-ratelimit-used"))
        {
          _logger.LogCritical("Reddit ratelimit used revoking access token!");
        }

        response.EnsureSuccessStatusCode();
      }
      catch (HttpRequestException e)
      {
        _logger.LogError(e, "Failed to revoke Reddit access token.");
      }
    }
  }

  /// <summary>
  /// https://github.com/reddit-archive/reddit/wiki/OAuth2#retrieving-the-access-token
  /// </summary>
  internal class AccessTokenResponse
  {
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;
  }
}
