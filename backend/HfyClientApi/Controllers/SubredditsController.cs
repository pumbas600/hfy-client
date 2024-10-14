using HfyClientApi.Dtos;
using HfyClientApi.Middleware;
using HfyClientApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HfyClientApi.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/v1/[controller]")]
  [EnableRateLimiting(RateLimiterPolicies.Authenticated)]
  public class SubredditsController : ControllerBase
  {
    private readonly ISubredditService _subredditService;

    public SubredditsController(ISubredditService subredditService)
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
