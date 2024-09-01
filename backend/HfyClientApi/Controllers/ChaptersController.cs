using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
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
      ChapterPaginationKey? nextKey = null;

      // TODO: Validate lastCreated is a UTC timestamp

      if (lastCreated != null && lastId != null)
      {
        nextKey = new ChapterPaginationKey()
        {
          // For some reason, C# doesn't realise that lastCreated is not null.
          LastCreatedAtUtc = lastCreated.Value.ToUniversalTime(),
          LastPostId = lastId,
        };
      }
      else if (lastCreated != null || lastId != null)
      {
        return Errors.ChapterPaginationPartialKeyset.ToActionResult();
      }

      var chapterPaginationDto = await _chapterService.GetPaginatedNewChaptersMetadataAsync(
        subreddit, pageSize, nextKey);

      return Ok(chapterPaginationDto);
    }

  }
}
