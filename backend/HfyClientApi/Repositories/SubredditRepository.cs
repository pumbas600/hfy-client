using HfyClientApi.Data;
using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Repositories
{
  public class SubredditRepository : ISubredditRepository
  {
    private readonly AppDbContext _context;

    public SubredditRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<Subreddit?> GetSubredditByNameAsync(string name)
    {
      var uppercaseName = name.ToUpper();
      return await _context.Subreddits
        .FirstOrDefaultAsync(s => s.Name.ToUpper() == uppercaseName);
    }
  }
}
