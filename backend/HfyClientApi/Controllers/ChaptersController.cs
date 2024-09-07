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

    [HttpGet("r/{subreddit}/new")]
    public async Task<ActionResult<ChapterPaginationDto>> GetNewSubredditChapters(
      [FromRoute] string subreddit, [FromQuery] DateTime? lastCreated,
      [FromQuery] string? lastId, [FromQuery] int pageSize = 20)
    {
      var nextKeyResult = ChapterPaginationKey.From(lastCreated, lastId);
      if (nextKeyResult.IsFailure)
      {
        return nextKeyResult.Error.ToActionResult();
      }

      var chapterPaginationDto = await _chapterService.GetPaginatedNewChaptersMetadataAsync(
        subreddit, pageSize, nextKeyResult.Data);

      return Ok(chapterPaginationDto);
    }

    [HttpGet("r/{subreddit}/search")]
    public async Task<ActionResult<ChapterPaginationDto>> GetNewSubredditChapters(
      [FromRoute] string subreddit, [FromQuery] string title, [FromQuery] DateTime? lastCreated,
      [FromQuery] string? lastId, [FromQuery] int pageSize = 20)
    {
      var nextKeyResult = ChapterPaginationKey.From(lastCreated, lastId);
      if (nextKeyResult.IsFailure)
      {
        return nextKeyResult.Error.ToActionResult();
      }

      var chapterPaginationDto = await _chapterService.GetPaginatedChaptersMetadataByTitleAsync(
        subreddit, title, pageSize, nextKeyResult.Data);

      return Ok(chapterPaginationDto);
    }

  }
}
