using HfyClientApi.Dtos;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public interface IUsersService
  {
    AuthorizationUrlDto GetAuthorizationUrl();

    Task<Result<LoginDto>> LoginWithRedditAsync(string redditAccessToken);

    Task<Result<UserDto>> GetUserByUsernameAsync(string username);
  }
}
