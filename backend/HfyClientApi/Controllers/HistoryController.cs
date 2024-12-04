using System.Security.Claims;
using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
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
  public class HistoryController : ControllerBase
  {
    private readonly IHistoryService _historyService;

    public HistoryController(IHistoryService historyService)
    {
      _historyService = historyService;
    }


    [HttpPost]
    public async Task<ActionResult<HistoryEntryDto>> ReadChapterById(
      [FromBody] CreateHistoryEntryDto createHistoryEntryDto)
    {
      var readerName = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (readerName == null)
      {
        return Errors.AuthSubjectMissing.ToActionResult();
      }

      var historyEntryResult = await _historyService.AddHistoryEntryAsync(
        createHistoryEntryDto.ChapterId, readerName
      );
      return historyEntryResult.ToActionResult(entry => StatusCode(201, entry));
    }
  }
}