using HfyClientApi.Dtos;

namespace HfyClientApi.Services
{
  public interface IUserService
  {
    AuthorizationUrlDto GetAuthorizationUrl();

    Task<LoginDto> LoginWithRedditAsync(string redditAccessToken);
  }
}
