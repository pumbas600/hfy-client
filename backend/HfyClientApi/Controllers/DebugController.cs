using HfyClientApi.Services;
using Microsoft.AspNetCore.Mvc;

#if DEBUG

namespace HfyClientApi.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class DebugController : ControllerBase
  {
    private readonly ICipherService _cipherService;

    public DebugController(ICipherService cipherService)
    {
      _cipherService = cipherService;
    }

    [HttpPost("decrypt")]
    public ActionResult<string> Decrypt([FromBody] string cipherText)
    {
      var result = _cipherService.Decrypt(cipherText);
      if (!result.IsSuccess)
      {
        return result.Error.ToActionResult();
      }

      return result.Data;
    }
  }
}

#endif
