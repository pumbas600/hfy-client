using HfyClientApi.Dtos;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public interface IUsersService
  {
    AuthorizationUrlDto GetAuthorizationUrl();

    Task<Result<LoginDto>> LoginWithRedditAsync(string redditCode);

    Task<Result<TokenPairDto>> RefreshAccessTokenAsync(string refreshToken);

    Task<Result<UserDto>> GetUserByUsernameAsync(string username);
  }
}
