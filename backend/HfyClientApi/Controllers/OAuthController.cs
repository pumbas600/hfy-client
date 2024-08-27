using Microsoft.AspNetCore.Mvc;

namespace HfyClientApi.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class OAuthController : ControllerBase
  {
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    public OAuthController(IHttpClientFactory clientFactory, IConfiguration configuration)
    {
      _clientFactory = clientFactory;
      _configuration = configuration;
    }

  }
}
