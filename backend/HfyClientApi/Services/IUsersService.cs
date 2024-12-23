using HfyClientApi.Dtos;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public interface IUsersService
  {
    AuthorizationUrlDto GetAuthorizationUrl();

    Task<Result<LoginDto>> LoginWithRedditAsync(string redditCode);

    Task<Result<LoginDto>> RefreshAccessTokenAsync(string refreshToken);

    Task LogoutAsync(string refreshToken);

    Task<Result<UserDto>> GetUserByUsernameAsync(string username);
  }
}
