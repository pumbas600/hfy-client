using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface IUsersRepository
  {
    Task<User?> GetUserByUsernameAsync(string username);

    Task<User> UpsertUserAsync(User user);
  }
}
