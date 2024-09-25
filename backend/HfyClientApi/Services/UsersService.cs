using System.Security.Cryptography;
using System.Text;
using System.Web;
using HfyClientApi.Configuration;
using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
using HfyClientApi.Models;
using HfyClientApi.Repositories;
using HfyClientApi.Utils;
using Reddit;

namespace HfyClientApi.Services
{
  public class UsersService : IUsersService
  {
    private readonly IUsersRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public UsersService(
      IUsersRepository userRepository, ITokenService tokenService,
      IConfiguration configuration, IMapper mapper)
    {
      _userRepository = userRepository;
      _tokenService = tokenService;
      _configuration = configuration;
      _mapper = mapper;
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

    public async Task<Result<UserDto>> GetUserByUsernameAsync(string username)
    {
      var user = await _userRepository.GetUserByUsernameAsync(username);
      if (user == null)
      {
        return Errors.UserNotFound(username);
      }

      return _mapper.ToUserDto(user);
    }

    public async Task<Result<LoginDto>> LoginWithRedditAsync(string redditAccessToken)
    {
      var reddit = new RedditClient(
        appId: _configuration[Config.Keys.RedditAppId],
        appSecret: _configuration[Config.Keys.RedditAppSecret],
        accessToken: redditAccessToken,
        userAgent: Config.UserAgent
      );

      Reddit.Controllers.User redditUser;
      try
      {
        redditUser = reddit.Account.GetMe();
      }
      catch (Reddit.Exceptions.RedditUnauthorizedException)
      {
        return Errors.AuthInvalidRedditAccessToken;
      }
      // TODO: Clean up access token by revoking it

      var user = new User()
      {
        Id = redditUser.Id,
        Name = redditUser.Name,
        IconUrl = redditUser.IconImg.Replace("&amp;", "&"), // Idk why Reddit does this 🤔
        SyncedAt = DateTime.UtcNow
      };

      await _userRepository.UpsertUserAsync(user);

      var accessToken = _tokenService.GenerateAccessToken(user.Name);
      var refreshToken = await _tokenService.GenerateNewRefreshTokenAsync(user.Name);

      return new LoginDto
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        User = _mapper.ToUserDto(user),
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
