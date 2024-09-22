using HfyClientApi.Data;
using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<User> UpsertUserAsync(User user)
    {
      await _context.Users.Upsert(user).RunAsync();
      await _context.SaveChangesAsync();

      return user;
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
      return await _context.Users.FindAsync(id);
    }
  }
}
