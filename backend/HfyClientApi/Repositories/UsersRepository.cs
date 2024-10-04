using HfyClientApi.Data;
using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Repositories
{
  public class UsersRepository : IUsersRepository
  {
    private readonly AppDbContext _context;

    public UsersRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<User?> UpsertUserAsync(User user)
    {
      await _context.Users.Upsert(user).RunAsync();
      await _context.SaveChangesAsync();

      return await GetUserByUsernameAsync(user.Name);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
      return await _context.Users
        .Include(u => u.WhitelistedUser)
        .FirstOrDefaultAsync(u => u.Name == username);
    }
  }
}
