using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
using HfyClientApi.Models;
using HfyClientApi.Repositories;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public class ChapterService : IChapterService
  {
    private readonly IChapterRepository _chapterRepository;
    private readonly IChapterParsingService _chapterParsingService;
    private readonly IRedditService _redditService;
    private readonly IMapper _mapper;

    public ChapterService(
      IChapterRepository chapterRepository, IChapterParsingService chapterParsingService,
      IRedditService redditService, IMapper mapper)
    {
      _chapterRepository = chapterRepository;
      _chapterParsingService = chapterParsingService;
      _redditService = redditService;
      _mapper = mapper;
    }

    public async Task<Result<FullChapterDto>> GetChapterByIdAsync(string id)
    {
      var chapterResult = await _chapterRepository.GetChapterByIdAsync(id);
      if (chapterResult.ErrorCode == Errors.Codes.ChapterNotFound)
      {
        return await ProcessChapterByIdAsync(id);
      }

      return chapterResult.Map(_mapper.ToFullChapterDto);
    }

    public async Task<ChapterPaginationDto> GetPaginatedNewChaptersMetadataAsync(
      string subreddit, int pageSize, ChapterPaginationKey? nextKey)
    {
      var chapters = await _chapterRepository.GetPaginatedNewChaptersMetadataAsync(
        subreddit, pageSize, nextKey);

      return _mapper.ToPaginatedChapterMetadataDto(pageSize, chapters);
    }

    public async Task<Result<FullChapterDto>> ProcessChapterByIdAsync(string id)
    {
      var selfPostResult = _redditService.GetSelfPostById(id);
      if (selfPostResult.IsFailure)
      {
        return selfPostResult.Error;
      }

      var selfPost = selfPostResult.Data;
      var parsedChapter = _chapterParsingService.ChapterFromPost(selfPost);

      // TODO: Check for broken/incorrect links between chapters.
      await UpdateChapterLinksAsync(parsedChapter);

      var createdChapterResult = await _chapterRepository.UpsertChapterAsync(parsedChapter);
      return createdChapterResult.Map(_mapper.ToFullChapterDto);

    }

    internal async Task<Result> UpdateChapterLinksAsync(Chapter parsedChapter)
    {
      // TODO: Does the update work as I've only selected a few columns?

      var (previousChapter, nextChapter) = await _chapterRepository.GetLinkedChaptersByChapterAsync(parsedChapter);
      if (previousChapter == null)
      {
        // TODO: Look for previous chapters with a next link to this chapter?
      }
      else if (previousChapter.NextChapterId == null)
      {
        previousChapter.NextChapterId = parsedChapter.Id;
        await _chapterRepository.UpdateChapterAsync(previousChapter, onlyLinks: true);
      }
      else if (previousChapter.NextChapterId != parsedChapter.Id)
      {
        // TODO: BROKEN LINK!
      }

      if (nextChapter == null)
      {
        // TODO: Look for next chapters with a previous link to this chapter?
      }
      else if (nextChapter.PreviousChapterId == null)
      {
        nextChapter.PreviousChapterId = parsedChapter.Id;
        await _chapterRepository.UpdateChapterAsync(nextChapter, onlyLinks: true);
      }
      else if (nextChapter.PreviousChapterId != parsedChapter.Id)
      {
        // TODO: BROKEN LINK!
      }

      return Result.Success();
    }
  }
}
