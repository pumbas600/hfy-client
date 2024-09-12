using HfyClientApi.Dtos;
using HfyClientApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HfyClientApi.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class SubredditController : ControllerBase
  {
    private readonly ISubredditService _subredditService;

    public SubredditController(ISubredditService subredditService)
    {
      _subredditService = subredditService;
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<SubredditDto>> GetSubredditByName([FromRoute] string name)
    {
      var subredditResult = await _subredditService.GetSubredditByNameAsync(name);
      return subredditResult.ToActionResult(Ok);
    }
  }
}
