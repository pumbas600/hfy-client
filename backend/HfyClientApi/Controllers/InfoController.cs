using HfyClientApi.Configuration;
using HfyClientApi.Dtos;
using HfyClientApi.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HfyClientApi.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  [EnableRateLimiting(RateLimiterPolicies.Public)]
  public class InfoController : ControllerBase
  {
    private readonly IWebHostEnvironment _environment;

    public InfoController(IWebHostEnvironment environment)
    {
      _environment = environment;
    }

    [HttpGet]
    public ActionResult<InfoDto> GetInfo()
    {
      var buildEnvironment = _environment.IsProduction()
        ? BuildEnvironment.Production
        : BuildEnvironment.Development;

      return Ok(new InfoDto()
      {
        Environment = buildEnvironment,
        ApiVersion = Config.ApiVersion
      });
    }
  }
}
