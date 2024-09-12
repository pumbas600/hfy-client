using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface ISubredditRepository
  {
    Task<Subreddit?> GetSubredditByNameAsync(string name);
  }
}
