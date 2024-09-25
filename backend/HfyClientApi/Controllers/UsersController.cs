using System.Security.Claims;
using HfyClientApi.Configuration;
using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
using HfyClientApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HfyClientApi.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class UsersController : ControllerBase
  {
    private readonly IUsersService _userService;

    public UsersController(IUsersService userService)
    {
      _userService = userService;
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
      [FromBody] RedditAccessTokenDto accessTokenDto)
    {
      var loginResultDto = await _userService.LoginWithRedditAsync(accessTokenDto.RedditAccessToken);
      if (!loginResultDto.IsSuccess)
      {
        return loginResultDto.Error.ToActionResult();
      }

      var loginDto = loginResultDto.Data;

      var accessTokenOptions = CreateCookieOptions(loginDto.AccessToken.ExpiresAt);
      var refreshTokenOptions = CreateCookieOptions(loginDto.RefreshToken.ExpiresAt);

      Response.Cookies.Append(Config.Cookies.AccessToken, loginDto.AccessToken.Value, accessTokenOptions);
      Response.Cookies.Append(Config.Cookies.RefreshToken, loginDto.RefreshToken.Value, refreshTokenOptions);

      return loginDto.User;
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshAccessToken()
    {
      return NoContent();
    }

    [HttpGet("@me")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<string>> GetSelf()
    {
      var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (username == null)
      {
        return Errors.AuthSubjectMissing.ToActionResult();
      }

      var userResult = await _userService.GetUserByUsernameAsync(username);
      return userResult.ToActionResult(Ok);
    }

    internal static CookieOptions CreateCookieOptions(DateTime expiresAt)
    {
      return new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = expiresAt
      };
    }
  }
}
