using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    private readonly JwtSettings _jwtSettings;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public UsersService(
      IUsersRepository userRepository, JwtSettings jwtSettings,
      IConfiguration configuration, IMapper mapper)
    {
      _userRepository = userRepository;
      _jwtSettings = jwtSettings;
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

    public async Task<LoginDto> LoginWithRedditAsync(string redditAccessToken)
    {
      var reddit = new RedditClient(
        appId: _configuration[Config.Keys.RedditAppId],
        appSecret: _configuration[Config.Keys.RedditAppSecret],
        accessToken: redditAccessToken,
        userAgent: Config.UserAgent
      );

      // TODO: Add try-catch block
      var redditUser = reddit.Account.GetMe();

      // TODO: Clean up access token by revoking it

      var user = new User()
      {
        Id = redditUser.Id,
        Name = redditUser.Name,
        IconUrl = redditUser.IconImg.Replace("&amp;", "&"), // Idk why Reddit does this 🤔
        SyncedAt = DateTime.UtcNow
      };

      var claims = new List<Claim> {
        new (ClaimTypes.NameIdentifier, user.Name),
      };

      await _userRepository.UpsertUserAsync(user);
      // var token = jwtSettings.CreateToken(claims);
      JwtSecurityToken token = null;

      return new LoginDto
      {
        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
        AccessTokenExpiresAt = token.ValidTo,
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
