namespace HfyClientApi.Configuration
{
  public static class Config
  {
    public class Keys
    {
      public const string Version = "Version";
      public const string RedditAppId = "RedditAppId";
      public const string RedditAppSecret = "RedditAppSecret";
      public const string RedditRefreshToken = "RedditRefreshToken";
      public const string RedditAccessToken = "RedditAccessToken";
      public const string RedditRedirectUri = "RedditRedirectUri";
    }

    public class Cookies
    {
      public const string AccessToken = "AccessToken";
      public const string RefreshToken = "RefreshToken";
      public const string UserProfile = "UserProfile";
    }

    public class Clients
    {
      public const string NoRedirect = "NoRedirect";
    }

    public const string RedditUrl = "https://www.reddit.com";
    public const string UnprefixedRedditUrl = "https://reddit.com";
    public const string OldRedditUrl = "https://old.reddit.com";
    public const string OauthRedditUrl = "https://oauth.reddit.com";

  }
}
