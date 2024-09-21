using HfyClientApi.Configuration;
using HfyClientApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HfyClientApi.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
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
