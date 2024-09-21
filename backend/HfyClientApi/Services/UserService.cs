using System.Security.Cryptography;
using System.Text;
using System.Web;
using HfyClientApi.Configuration;
using HfyClientApi.Dtos;

namespace HfyClientApi.Services
{
  public class UserService : IUserService
  {
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public AuthorizationUrlDto GetAuthorizationUrl()
    {
      var appId = _configuration[Config.Keys.RedditAppId];
      var redirectUrl = _configuration[Config.Keys.RedditRedirectUri];
      var scope = "identity";
      var state = RandomString(64);

      return new AuthorizationUrlDto()
      {
        Url = Config.RedditUrl + "/api/v1/authorize?client_id=" + appId + "&response_type=code"
          + "&state=" + state
          + "&redirect_uri=" + redirectUrl
          + "&scope=" + scope
      };
    }

    internal static string RandomString(int length)
    {
      return HttpUtility.UrlEncode(
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(length)),
        Encoding.UTF8
      );
    }
  }
}
