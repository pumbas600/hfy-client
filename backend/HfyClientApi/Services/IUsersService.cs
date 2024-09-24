using HfyClientApi.Dtos;

namespace HfyClientApi.Services
{
  public interface IUsersService
  {
    AuthorizationUrlDto GetAuthorizationUrl();

    Task<LoginDto> LoginWithRedditAsync(string redditAccessToken);
  }
}
