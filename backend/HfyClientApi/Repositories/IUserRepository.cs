using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface IUserRepository
  {
    Task<User?> GetUserByIdAsync(string id);

    Task<User> UpsertUserAsync(User user);
  }
}
