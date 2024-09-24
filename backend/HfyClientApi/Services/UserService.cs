using System.Security.Cryptography;
using System.Text;
using System.Web;
using HfyClientApi.Configuration;
using HfyClientApi.Dtos;
using HfyClientApi.Models;
using HfyClientApi.Repositories;
using Reddit;

namespace HfyClientApi.Services
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
      _userRepository = userRepository;
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
          + "&scope=" + scope,
        State = state
      };
    }

    public async void LoginWithReddit(string accessToken)
    {
      var reddit = new RedditClient(
        appId: _configuration[Config.Keys.RedditAppId],
        appSecret: _configuration[Config.Keys.RedditAppSecret],
        accessToken: accessToken,
        userAgent: Config.UserAgent
      );

      // TODO: Add try-catch block
      var redditUser = reddit.Account.GetMe();

      // TODO: Clean up access token by revoking it

      var user = new User()
      {
        Id = redditUser.Id,
        Name = redditUser.Name,
        IconUrl = redditUser.IconImg,
        SyncedAt = DateTime.UtcNow
      };

      await _userRepository.UpsertUserAsync(user);
      // TODO: Generate JWT token
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
