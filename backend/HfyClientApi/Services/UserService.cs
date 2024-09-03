
using HfyClientApi.Configuration;
using HfyClientApi.Dtos.External;
using HfyClientApi.Exceptions;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public class UserService : IUserService
  {
    private IHttpClientFactory _clientFactory;

    public UserService(IHttpClientFactory clientFactory)
    {
      _clientFactory = clientFactory;
    }

    public async Task<Result<string>> GetUserProfilePictureUrlAsync(string username)
    {
      var client = _clientFactory.CreateClient(Config.HttpClients.Reddit);
      var aboutUser = await client.GetFromJsonAsync<AboutUserDto>($"/user/{username}/about.json");

      if (aboutUser == null)
      {
        return Errors.UserNotFound(username);
      }

      return aboutUser.IconImg;
    }
  }
}
