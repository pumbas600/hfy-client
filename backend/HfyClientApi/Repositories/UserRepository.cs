using HfyClientApi.Data;
using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
      _context = context;
    }

    public Task<User> CreateUserAsync(User user)
    {
      throw new NotImplementedException();
    }

    public Task<User?> GetUserByIdAsync(string id)
    {
      throw new NotImplementedException();
    }
  }
}
