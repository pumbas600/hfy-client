using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
using HfyClientApi.Repositories;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public class SubredditService : ISubredditService
  {
    private readonly ISubredditRepository _subredditRepository;
    private readonly IMapper _mapper;

    public SubredditService(ISubredditRepository subredditRepository, IMapper mapper)
    {
      _subredditRepository = subredditRepository;
      _mapper = mapper;
    }

    public async Task<Result<SubredditDto>> GetSubredditByNameAsync(string name)
    {
      var subreddit = await _subredditRepository.GetSubredditByNameAsync(name);
      if (subreddit == null)
      {
        return Errors.SubredditNotFound(name);
      }

      return _mapper.ToSubredditDto(subreddit);
    }
  }
}
