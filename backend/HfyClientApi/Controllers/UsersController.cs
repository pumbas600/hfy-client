using System.Security.Claims;
using System.Text;
using System.Text.Json;
using HfyClientApi.Configuration;
using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
using HfyClientApi.Middleware;
using HfyClientApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HfyClientApi.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  [EnableRateLimiting(RateLimiterPolicies.PublicLogin)]
  public class UsersController : ControllerBase
  {
    private readonly ICipherService _cipherService;
    private readonly IUsersService _userService;
    private readonly JwtSettings _jwtSettings;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public UsersController(
      ICipherService cipherService, IUsersService userService, JwtSettings jwtSettings)
    {
      _cipherService = cipherService;
      _userService = userService;
      _jwtSettings = jwtSettings;
    }

    [AllowAnonymous]
    [HttpGet("reddit/authorize")]
    [EnableRateLimiting(RateLimiterPolicies.Public)]
    public ActionResult<AuthorizationUrlDto> GetAuthorizationUrl()
    {
      return _userService.GetAuthorizationUrl();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginWithReddit(
      [FromBody] RedditCodeDto redditCodeDto)
    {
      var loginResultDto = await _userService.LoginWithRedditAsync(redditCodeDto.RedditCode);
      if (!loginResultDto.IsSuccess)
      {
        return loginResultDto.Error.ToActionResult();
      }

      var loginDto = loginResultDto.Data;

      SetCookies(loginDto);

      return Ok(loginDto.User);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshAccessToken()
    {
      if (!Request.Cookies.TryGetValue(Config.Cookies.RefreshToken, out var refreshToken))
      {
        return Errors.AuthRefreshTokenMissing.ToActionResult();
      }

      var loginResultDto = await _userService.RefreshAccessTokenAsync(refreshToken);
      if (!loginResultDto.IsSuccess)
      {
        return loginResultDto.Error.ToActionResult();
      }

      SetCookies(loginResultDto.Data);

      return NoContent();
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
      if (Request.Cookies.TryGetValue(Config.Cookies.RefreshToken, out var refreshToken))
      {
        await _userService.LogoutAsync(refreshToken);
      }

      Response.Cookies.Delete(Config.Cookies.AccessToken);
      Response.Cookies.Delete(Config.Cookies.RefreshToken);
      Response.Cookies.Delete(Config.Cookies.UserProfile);

      return NoContent();
    }

    [Authorize]
    [HttpGet("@me")]
    [EnableRateLimiting(RateLimiterPolicies.Authenticated)]
    public async Task<ActionResult<UserDto>> GetSelf()
    {
      var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (username == null)
      {
        return Errors.AuthSubjectMissing.ToActionResult();
      }

      var userResult = await _userService.GetUserByUsernameAsync(username);
      return userResult.ToActionResult(Ok);
    }

    internal void SetCookies(LoginDto loginDto)
    {
      var accessTokenOptions = CreateCookieOptions(loginDto.AccessToken.ExpiresAt);
      var refreshTokenOptions = CreateCookieOptions(loginDto.RefreshToken.ExpiresAt);
      var userProfileOptions = CreateCookieOptions(loginDto.RefreshToken.ExpiresAt, false);

      var accessToken = _cipherService.Encrypt(loginDto.AccessToken.Value);
      var refreshToken = loginDto.RefreshToken.Value;
      var userProfile = SerializeUserProfileCookieValue(loginDto.User);

      Response.Cookies.Append(Config.Cookies.AccessToken, accessToken, accessTokenOptions);
      Response.Cookies.Append(Config.Cookies.RefreshToken, refreshToken, refreshTokenOptions);
      Response.Cookies.Append(Config.Cookies.UserProfile, userProfile, userProfileOptions);

    }

    internal string SerializeUserProfileCookieValue(UserDto userDto)
    {
      var userProfile = JsonSerializer.Serialize(userDto, _jsonSerializerOptions);
      return Convert.ToBase64String(Encoding.UTF8.GetBytes(userProfile));
    }

    internal CookieOptions CreateCookieOptions(DateTime expiresAt, bool httpOnly = true)
    {
      var cookieExpiresAt = expiresAt.AddMinutes(-_jwtSettings.CookieEarlyExpirationOffsetMinutes);
      return new CookieOptions
      {
        HttpOnly = httpOnly,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = cookieExpiresAt,
        IsEssential = true,
      };
    }
  }
}
