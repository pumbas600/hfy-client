using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HfyClientApi.Configuration
{
  public static class Config
  {
    public class Keys
    {
      public const string RedditAppId = "RedditAppId";
      public const string RedditAppSecret = "RedditAppSecret";
      public const string RedditRefreshToken = "RedditRefreshToken";
      public const string RedditAccessToken = "RedditAccessToken";
      public const string RedditRedirectUri = "RedditRedirectUri";
      public const string JwtIssuer = "JwtIssuer";
      public const string JwtAudience = "JwtAudience";
      public const string JwtKey = "JwtKey";
    }

    public class Jwt
    {
      public string Issuer { get; private set; }
      public string Audience { get; private set; }
      public int ExpiresInMinutes { get; private set; }
      public SymmetricSecurityKey SigningKey { get; private set; }

      public Jwt(IConfiguration configuration)
      {
        Issuer = configuration[Keys.JwtIssuer]!;
        Audience = configuration[Keys.JwtAudience]!;
        ExpiresInMinutes = 15;
        SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[Keys.JwtKey]!));
      }

      public JwtSecurityToken CreateToken()
      {
        return new JwtSecurityToken(
          issuer: Issuer,
          audience: Audience,
          expires: DateTime.UtcNow.AddMinutes(ExpiresInMinutes),
          signingCredentials: new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256)
        );
      }
    }

    public class Cookies
    {
      public const string AccessToken = "AccessToken";
      public const string RefreshToken = "RefreshToken";
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
