using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
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
      int pageSize, ChapterPaginationKey? nextKey)
    {
      var chapters = await _chapterRepository.GetPaginatedNewChaptersMetadataAsync(
        pageSize, nextKey);

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

      var createdChapterResult = await _chapterRepository.UpsertChapterAsync(parsedChapter);
      return createdChapterResult.Map(_mapper.ToFullChapterDto);

    }
  }
}
