using HfyClientApi.Dtos;
using HfyClientApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HfyClientApi.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class ChaptersController : ControllerBase
  {
    private readonly IChapterService _chapterService;

    public ChaptersController(IChapterService chapterService)
    {
      _chapterService = chapterService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FullChapterDto>> GetChapterByIdAsync([FromRoute] string id)
    {
      var chapter = await _chapterService.GetChapterByIdAsync(id);
      return chapter;
    }

    [HttpPut("{id}/process")]
    public async Task<ActionResult<FullChapterDto>> ProcessChapterByIdAsync([FromRoute] string id)
    {
      var chapter = await _chapterService.ProcessChapterByIdAsync(id);
      return chapter;
    }
  }
}
