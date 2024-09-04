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
    private readonly IStoryMetadataRepository _storyMetadataRepository;
    private readonly IChapterParsingService _chapterParsingService;
    private readonly IRedditService _redditService;
    private readonly ILogger<ChapterService> _logger;
    private readonly IMapper _mapper;

    public ChapterService(
      IChapterRepository chapterRepository, IChapterParsingService chapterParsingService,
      IStoryMetadataRepository storyMetadataRepository, IRedditService redditService,
      ILogger<ChapterService> logger, IMapper mapper)
    {
      _chapterRepository = chapterRepository;
      _storyMetadataRepository = storyMetadataRepository;
      _chapterParsingService = chapterParsingService;
      _redditService = redditService;
      _logger = logger;
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
      var (parsedChapter, storyMetadata) = await _chapterParsingService.ChapterFromPost(selfPost);

      await UpdateChapterLinksAsync(parsedChapter);

      var createdChapterResult = await _chapterRepository.UpsertChapterAsync(parsedChapter);
      if (createdChapterResult.IsFailure)
      {
        return createdChapterResult.Error;
      }

      var createdChapter = createdChapterResult.Data;


      if (storyMetadata != null)
      {
        await _storyMetadataRepository.UpsertMetadata(storyMetadata);
      }
      else if (createdChapter.FirstChapterId != null)
      {
        storyMetadata = await _storyMetadataRepository.GetMetadata(createdChapter.FirstChapterId);
      }

      return _mapper.ToFullChapterDto(new CombinedChapter
      {
        Chapter = createdChapter,
        StoryMetadata = storyMetadata
      });
    }

    internal async Task<Result> UpdateChapterLinksAsync(Chapter chapter)
    {
      var (previousChapter, nextChapter) = await _chapterRepository.GetLinkedChaptersByChapterAsync(chapter);

      await UpdatePreviousChapterLinkAsync(chapter, previousChapter);
      await UpdateNextChapterLinkAsync(chapter, nextChapter);

      return Result.Success();
    }

    internal async Task UpdatePreviousChapterLinkAsync(
      Chapter originalChapter, Chapter? previousChapter)
    {
      if (previousChapter == null)
      {
        // Find a chapter with a next link to the original chapter.
        previousChapter = await _chapterRepository.GetChapterByNextLinkIdAsync(originalChapter.Id);
        if (previousChapter != null)
        {
          originalChapter.PreviousChapterId = previousChapter.Id;
        }
      }
      else if (previousChapter.NextChapterId == null)
      {
        previousChapter.NextChapterId = originalChapter.Id;
        await _chapterRepository.UpdateChapterAsync(previousChapter, onlyLinks: true);
      }
      else if (previousChapter.NextChapterId != originalChapter.Id)
      {
        _logger.LogWarning(
          "Chapter {} has a broken previous link. The linked chapter has a next link to {}",
          originalChapter.Id, previousChapter.NextChapterId
        );
      }
    }

    internal async Task UpdateNextChapterLinkAsync(Chapter originalChapter, Chapter? nextChapter)
    {
      if (nextChapter == null)
      {
        // Find a chapter with a previous link to the original chapter.
        nextChapter = await _chapterRepository.GetChapterByPreviousLinkIdAsync(originalChapter.Id);
        if (nextChapter != null)
        {
          originalChapter.NextChapterId = nextChapter.Id;
        }
      }
      else if (nextChapter.PreviousChapterId == null)
      {
        nextChapter.PreviousChapterId = originalChapter.Id;
        await _chapterRepository.UpdateChapterAsync(nextChapter, onlyLinks: true);
      }
      else if (nextChapter.PreviousChapterId != originalChapter.Id)
      {
        _logger.LogWarning(
          "Chapter {} has a broken next link. The linked chapter has a previous link to {}",
          originalChapter.Id, nextChapter.PreviousChapterId
        );
      }
    }
  }
}
