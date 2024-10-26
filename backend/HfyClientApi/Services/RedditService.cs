using System.Text;
using System.Text.Json.Serialization;
using HfyClientApi.Configuration;
using HfyClientApi.Exceptions;
using HfyClientApi.Utils;
using Reddit;
using Reddit.Controllers;
using Reddit.Exceptions;

namespace HfyClientApi.Services
{
  public class RedditService : IRedditService
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RedditClient _redditClient;
    private readonly ILogger<RedditService> _logger;
    private readonly IConfiguration _configuration;
    private readonly VersionSettings _versionSettings;

    public RedditService(
      IHttpClientFactory httpClientFactory, RedditClient redditClient,
      ILogger<RedditService> logger, IConfiguration configuration,
      VersionSettings versionSettings)
    {
      _httpClientFactory = httpClientFactory;
      _redditClient = redditClient;
      _logger = logger;
      _configuration = configuration;
      _versionSettings = versionSettings;
    }

    public async Task<Result<string>> GetAccessTokenAsync(string code)
    {
      using HttpClient client = _httpClientFactory.CreateClient();

      var redirectUri = _configuration[Config.Keys.RedditRedirectUri]!;
      var appId = _configuration[Config.Keys.RedditAppId]!;
      var appSecret = _configuration[Config.Keys.RedditAppSecret]!;
      var base64Token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{appId}:{appSecret}"));

      var request = new HttpRequestMessage
      {
        Method = HttpMethod.Post,
        RequestUri = new Uri(Config.RedditUrl + "/api/v1/access_token"),
        Headers = {
          { "User-Agent", _versionSettings.UserAgent },
          { "Authorization", $"Basic {base64Token}" },
        },
        Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
          { "grant_type", "authorization_code" },
          { "code", code },
          { "redirect_uri", redirectUri },
        }),
      };

      try
      {
        var response = await client.SendAsync(request);
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
        _logger.LogDebug(e, "Failed to exchange code for access token.");
        return Errors.AuthCodeExchangeError;
      }
    }

    public IEnumerable<SelfPost> GetNewSelfPosts(string subreddit, int limit = 50)
    {
      try
      {
        return _redditClient.Subreddit(subreddit).Posts
          .GetNew(limit: limit)
          .Where(p => p is SelfPost)
          .Cast<SelfPost>();
      }
      catch (RedditException e)
      {
        _logger.LogError(e, "Failed to get new posts from Reddit.");
        return [];
      }
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

      var request = new HttpRequestMessage
      {
        Method = HttpMethod.Post,
        RequestUri = new Uri(Config.RedditUrl + "/api/v1/revoke_token"),
        Headers = {
          { "User-Agent", _versionSettings.UserAgent },
        },
        Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
          { "token", accessToken },
          { "token_type_hint", "access_token" }
        }),
      };

      try
      {
        var response = await client.SendAsync(request);
        if (response.Headers.Contains("x-ratelimit-used"))
        {
          _logger.LogCritical("Reddit ratelimit used revoking access token!");
        }

        response.EnsureSuccessStatusCode();
      }
      catch (HttpRequestException e)
      {
        _logger.LogDebug(e, "Failed to revoke Reddit access token.");
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
