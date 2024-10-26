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
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRedditService _redditService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly ILogger<UsersService> _logger;
    private readonly VersionSettings _versionSettings;

    public UsersService(
      IUsersRepository userRepository, ITokenService tokenService,
      IRefreshTokenRepository refreshTokenRepository, IRedditService redditService,
      IConfiguration configuration, IMapper mapper, ILogger<UsersService> logger,
      VersionSettings versionSettings)
    {
      _userRepository = userRepository;
      _tokenService = tokenService;
      _refreshTokenRepository = refreshTokenRepository;
      _redditService = redditService;
      _configuration = configuration;
      _mapper = mapper;
      _logger = logger;
      _versionSettings = versionSettings;
    }

    public AuthorizationUrlDto GetAuthorizationUrl()
    {
      var appId = _configuration[Config.Keys.RedditAppId];
      var redirectUrl = _configuration[Config.Keys.RedditRedirectUri];
      var scope = "identity";

      return new()
      {
        Url = Config.RedditUrl + "/api/v1/authorize?client_id=" + appId + "&response_type=code"
          + "&redirect_uri=" + HttpUtility.UrlEncode(redirectUrl, Encoding.UTF8)
          + "&scope=" + scope
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

    public async Task<Result<LoginDto>> LoginWithRedditAsync(string redditCode)
    {
      var accessTokenResult = await _redditService.GetAccessTokenAsync(redditCode);
      if (accessTokenResult.IsFailure)
      {
        return accessTokenResult.Error;
      }

      var redditAccessToken = accessTokenResult.Data;

      var reddit = new RedditClient(
        appId: _configuration[Config.Keys.RedditAppId],
        appSecret: _configuration[Config.Keys.RedditAppSecret],
        accessToken: redditAccessToken,
        userAgent: _versionSettings.UserAgent
      );

      Reddit.Controllers.User redditUser;
      try
      {
        redditUser = reddit.Account.GetMe();
      }
      catch (Reddit.Exceptions.RedditException e)
      {
        _logger.LogError(e, "Failed to get Reddit self with access token");
        return Errors.AuthInvalidRedditAccessToken;
      }

      await _redditService.RevokeAccessTokenAsync(redditAccessToken);

      var user = new User()
      {
        Id = redditUser.Id,
        Name = redditUser.Name,
        IconUrl = redditUser.IconImg.Replace("&amp;", "&"), // Idk why Reddit does this 🤔
        SyncedAt = DateTime.UtcNow
      };

      var upsertedUser = await _userRepository.UpsertUserAsync(user);
      if (upsertedUser?.WhitelistedUser == null)
      {
        return Errors.UserNotWhitelisted(user.Name);
      }

      var accessToken = _tokenService.GenerateAccessToken(user.Name);
      var refreshToken = _tokenService.GenerateRefreshToken(user.Name);

      var storedRefreshToken = new RefreshToken()
      {
        Token = HashRefreshToken(refreshToken.Value),
        ExpiresAt = refreshToken.ExpiresAt,
        Username = user.Name,
      };

      await _refreshTokenRepository.SaveRefreshTokenAsync(storedRefreshToken);

      return new LoginDto
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        User = _mapper.ToUserDto(user),
      };
    }

    public async Task LogoutAsync(string refreshToken)
    {
      await _refreshTokenRepository.DeleteRefreshTokenAsync(HashRefreshToken(refreshToken));
    }

    public async Task<Result<LoginDto>> RefreshAccessTokenAsync(string refreshToken)
    {
      var hashedRefreshToken = HashRefreshToken(refreshToken);
      var storedRefreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(hashedRefreshToken);
      if (storedRefreshToken == null)
      {
        return Errors.AuthInvalidRefreshToken;
      }

      if (storedRefreshToken.ExpiresAt < DateTime.UtcNow)
      {
        return Errors.AuthExpiredRefreshToken;
      }

      var username = storedRefreshToken.Username;

      var accessToken = _tokenService.GenerateAccessToken(username);
      var newRefreshToken = _tokenService.GenerateRefreshToken(username);

      storedRefreshToken.Token = HashRefreshToken(newRefreshToken.Value);
      storedRefreshToken.ExpiresAt = newRefreshToken.ExpiresAt;

      await _refreshTokenRepository.UpdateRefreshTokenAsync(storedRefreshToken);
      var user = await _userRepository.GetUserByUsernameAsync(username);

      if (user == null)
      {
        return Errors.UserNotFound(username);
      }

      if (user.WhitelistedUser == null)
      {
        return Errors.UserNotWhitelisted(username);
      }

      var loginDto = new LoginDto()
      {
        AccessToken = accessToken,
        RefreshToken = newRefreshToken,
        User = _mapper.ToUserDto(user),
      };

      return loginDto;
    }

    internal static string HashRefreshToken(string refreshToken)
    {
      using var sha256 = SHA256.Create();
      var hash = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
      return Convert.ToBase64String(hash);
    }
  }
}
