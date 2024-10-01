using System.Security.Claims;
using HfyClientApi.Configuration;
using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
using HfyClientApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HfyClientApi.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class UsersController : ControllerBase
  {
    private readonly IUsersService _userService;
    private readonly JwtSettings _jwtSettings;

    public UsersController(IUsersService userService, JwtSettings jwtSettings)
    {
      _userService = userService;
      _jwtSettings = jwtSettings;
    }

    [AllowAnonymous]
    [HttpGet("reddit/authorize")]
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

      SetTokenCookies(loginDto);

      return loginDto.User;
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshAccessToken()
    {
      if (!Request.Cookies.TryGetValue(Config.Cookies.RefreshToken, out var refreshToken))
      {
        return Errors.AuthRefreshTokenMissing.ToActionResult();
      }

      var refreshResultDto = await _userService.RefreshAccessTokenAsync(refreshToken);
      if (!refreshResultDto.IsSuccess)
      {
        return refreshResultDto.Error.ToActionResult();
      }

      SetTokenCookies(refreshResultDto.Data);

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

      return NoContent();
    }

    [Authorize]
    [HttpGet("@me")]
    public async Task<ActionResult<LoginDto>> GetSelf()
    {
      var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (username == null)
      {
        return Errors.AuthSubjectMissing.ToActionResult();
      }

      var userResult = await _userService.GetUserByUsernameAsync(username);
      return userResult.ToActionResult(Ok);
    }

    internal void SetTokenCookies(TokenPairDto tokenPair)
    {
      var accessTokenOptions = CreateCookieOptions(tokenPair.AccessToken.ExpiresAt);
      var refreshTokenOptions = CreateCookieOptions(tokenPair.RefreshToken.ExpiresAt);

      Response.Cookies.Append(Config.Cookies.AccessToken, tokenPair.AccessToken.Value, accessTokenOptions);
      Response.Cookies.Append(Config.Cookies.RefreshToken, tokenPair.RefreshToken.Value, refreshTokenOptions);

    }

    internal CookieOptions CreateCookieOptions(DateTime expiresAt)
    {
      var cookieExpiresAt = expiresAt.AddMinutes(_jwtSettings.CookieEarlyExpirationOffsetMinutes);
      return new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = cookieExpiresAt,
        IsEssential = true,
      };
    }
  }
}
