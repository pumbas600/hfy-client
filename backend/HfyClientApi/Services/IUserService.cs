using HfyClientApi.Dtos;

namespace HfyClientApi.Services
{
  public interface IUserService
  {
    AuthorizationUrlDto GetAuthorizationUrl();
  }
}
