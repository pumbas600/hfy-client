using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public interface IUserService
  {
    Task<Result<string>> GetUserProfilePictureUrlAsync(string username);
  }
}
