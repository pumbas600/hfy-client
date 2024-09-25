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
    [HttpPost("reddit/login")]
    public async Task<ActionResult<UserDto>> LoginWithReddit([FromBody] string accessToken)
    {
      var loginDto = await _userService.LoginWithRedditAsync(accessToken);
      var options = new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = loginDto.AccessToken.ExpiresAt
      };

      Response.Cookies.Append(Config.Cookies.AccessToken, loginDto.AccessToken.Value, options);

      return loginDto.User;
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
  }
}
