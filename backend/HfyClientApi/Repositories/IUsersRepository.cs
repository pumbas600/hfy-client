using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface IUsersRepository
  {
    Task<User?> GetUserByIdAsync(string id);

    Task<User> UpsertUserAsync(User user);
  }
}
