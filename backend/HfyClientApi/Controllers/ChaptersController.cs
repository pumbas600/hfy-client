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
    public async Task<ActionResult<FullChapterDto>> GetChapterById([FromRoute] string id)
    {
      var chapterResult = await _chapterService.GetChapterByIdAsync(id);
      return chapterResult.ToActionResult(Ok);
    }

    [HttpPut("{id}/process")]
    public async Task<ActionResult<FullChapterDto>> ProcessChapterById([FromRoute] string id)
    {
      var chapterResult = await _chapterService.ProcessChapterByIdAsync(id);

      return chapterResult.ToActionResult(
        chapter => CreatedAtAction(nameof(GetChapterById), new { id = chapter.Id }, chapter)
      );
    }
  }
}
