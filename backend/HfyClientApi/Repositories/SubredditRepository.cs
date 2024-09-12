using HfyClientApi.Data;
using HfyClientApi.Models;

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
      return await _context.Subreddits.FindAsync(name);
    }
  }
}
