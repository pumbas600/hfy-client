namespace HfyClientApi.Configuration
{
  public class Config
  {
    public class Keys
    {
      public const string RedditAppId = "RedditAppId";
      public const string RedditAppSecret = "RedditAppSecret";
      public const string RedditRefreshToken = "RedditRefreshToken";
      public const string RedditAccessToken = "RedditAccessToken";
      public const string RedditRedirectUri = "RedditRedirectUri";
    }

    public class Clients
    {
      public const string NoRedirect = "NoRedirect";
    }

    public const string ApiVersion = "v0.0.1";
    public const string UserAgent = $"hfy-client/{ApiVersion}";
    public const string RedditUrl = "https://www.reddit.com";
    public const string UnprefixedRedditUrl = "https://reddit.com";
    public const string OldRedditUrl = "https://old.reddit.com";
    public const string OauthRedditUrl = "https://oauth.reddit.com";

  }
}
