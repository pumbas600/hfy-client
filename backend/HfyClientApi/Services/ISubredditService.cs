using HfyClientApi.Dtos;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public interface ISubredditService
  {
    Task<Result<SubredditDto>> GetSubredditByNameAsync(string name);
  }
}
