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

    [HttpGet("r/{subreddit}/new")]
    public async Task<ActionResult<ChapterPaginationDto>> GetNewSubredditChapters(
      [FromRoute] string subreddit, [FromQuery] DateTime? lastCreated,
      [FromQuery] string? lastId, [FromQuery] int pageSize = 20, [FromQuery] string? title = null)
    {
      var nextKeyResult = ChapterPaginationKey.From(lastCreated, lastId);
      if (nextKeyResult.IsFailure)
      {
        return nextKeyResult.Error.ToActionResult();
      }

      var chapterPaginationDto = await _chapterService.GetPaginatedNewChaptersMetadataAsync(
        subreddit, title, pageSize, nextKeyResult.Data);

      return Ok(chapterPaginationDto);
    }
  }
}
