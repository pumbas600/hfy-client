using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
using HfyClientApi.Models;
using HfyClientApi.Repositories;
using HfyClientApi.Utils;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public class ChapterService : IChapterService
  {
    private readonly IChapterRepository _chapterRepository;
    private readonly IHistoryEntriesRepository _historyEntriesRepository;
    private readonly IStoryMetadataRepository _storyMetadataRepository;
    private readonly IChapterParsingService _chapterParsingService;
    private readonly IRedditService _redditService;
    private readonly ILogger<ChapterService> _logger;
    private readonly IMapper _mapper;

    public ChapterService(
      IChapterRepository chapterRepository, IChapterParsingService chapterParsingService,
      IHistoryEntriesRepository historyEntriesRepository,
      IStoryMetadataRepository storyMetadataRepository, IRedditService redditService,
      ILogger<ChapterService> logger, IMapper mapper)
    {
      _chapterRepository = chapterRepository;
      _historyEntriesRepository = historyEntriesRepository;
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


    public async Task<Result<HistoryEntryDto>> ReadChapterByIdAsync(string id, string readerName)
    {
      var newHistoryEntry = new HistoryEntry
      {
        ChapterId = id,
        UserName = readerName,
        ReadAtUtc = DateTime.UtcNow,
      };

      var createdHistoryEntry = await _historyEntriesRepository.AddHistoryEntryAsync(newHistoryEntry);
      return _mapper.ToHistoryEntryDto(createdHistoryEntry);
    }

    public async Task<ChapterPaginationDto> GetPaginatedNewChaptersMetadataAsync(
      string subreddit, string? title, int pageSize, ChapterPaginationKey? nextKey)
    {
      IEnumerable<CombinedChapter> chapters;
      if (title != null)
      {
        chapters = await _chapterRepository.GetPaginatedChaptersMetadataByTitleAsync(
          subreddit, title, pageSize, nextKey);
      }
      else
      {
        chapters = await _chapterRepository.GetPaginatedNewChaptersMetadataAsync(
          subreddit, pageSize, nextKey);
      }
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
      return await ProcessChapterByPostAsync(selfPost);
    }

    public async Task ProcessChaptersByPostAsync(IEnumerable<SelfPost> posts)
    {
      var chapters = await _chapterRepository.GetChaptersByIdsAsync(posts.Select(post => post.Id));
      var chapterMap = chapters.ToDictionary(c => c.Id);

      foreach (var post in posts)
      {
        if (chapterMap.TryGetValue(post.Id, out Chapter? chapter))
        {
          await UpdateChapterAsync(chapter, post);
        }
        else
        {
          await CreateChapterAsync(post);
        }
      }
    }

    internal async Task UpdateChapterAsync(Chapter chapter, SelfPost post)
    {
      var postEditedAtUtc = _chapterParsingService.GetEditedAtUtc(post);
      if (chapter.EditedAtUtc == postEditedAtUtc)
      {
        if (chapter.Upvotes != post.UpVotes || chapter.Downvotes != post.DownVotes)
        {
          await _chapterRepository.UpdateChapterAsync(chapter);
        }
        return;
      }

      _logger.LogInformation(
        "Updating chapter {}. Post was edited at {}. Chapter was edited at {}",
        chapter.Id, postEditedAtUtc, chapter.EditedAtUtc);

      var (parsedChapter, storyMetadata) = await _chapterParsingService.ChapterFromPostAsync(post);
      await UpdateChapterLinksAsync(parsedChapter);

      if (storyMetadata != null)
      {
        await _storyMetadataRepository.UpsertMetadataAsync(storyMetadata);
      }

      await _chapterRepository.UpdateChapterAsync(parsedChapter);
    }

    internal async Task CreateChapterAsync(SelfPost post)
    {
      var (parsedChapter, storyMetadata) = await _chapterParsingService.ChapterFromPostAsync(post);
      await UpdateChapterLinksAsync(parsedChapter);

      if (storyMetadata != null)
      {
        await _storyMetadataRepository.UpsertMetadataAsync(storyMetadata);
      }

      await _chapterRepository.CreateChapterAsync(parsedChapter);
    }

    public async Task<Result<FullChapterDto>> ProcessChapterByPostAsync(SelfPost post)
    {
      var (parsedChapter, storyMetadata) = await _chapterParsingService.ChapterFromPostAsync(post);

      await UpdateChapterLinksAsync(parsedChapter);

      var createdChapterResult = await _chapterRepository.UpsertChapterAsync(parsedChapter);
      if (createdChapterResult.IsFailure)
      {
        return createdChapterResult.Error;
      }

      var createdChapter = createdChapterResult.Data;


      if (storyMetadata != null)
      {
        await _storyMetadataRepository.UpsertMetadataAsync(storyMetadata);
      }
      else if (createdChapter.FirstChapterId != null)
      {
        storyMetadata = await _storyMetadataRepository.GetMetadataAsync(createdChapter.FirstChapterId);
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
      bool isPreviousChapterUpdated = false;
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
        isPreviousChapterUpdated = true;
      }
      else if (previousChapter.NextChapterId != originalChapter.Id)
      {
        _logger.LogWarning(
          "Chapter {} has a broken previous link. The linked chapter has a next link to {}",
          originalChapter.Id, previousChapter.NextChapterId
        );
      }

      if (previousChapter != null)
      {
        isPreviousChapterUpdated = isPreviousChapterUpdated || UpdateFirstLink(originalChapter, previousChapter);

        if (isPreviousChapterUpdated)
        {
          await _chapterRepository.UpdateChapterAsync(previousChapter, onlyLinks: true);
        }
      }
    }

    internal async Task UpdateNextChapterLinkAsync(Chapter originalChapter, Chapter? nextChapter)
    {
      bool isNextChapterUpdated = false;
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
        isNextChapterUpdated = true;
      }
      else if (nextChapter.PreviousChapterId != originalChapter.Id)
      {
        _logger.LogWarning(
          "Chapter {} has a broken next link. The linked chapter has a previous link to {}",
          originalChapter.Id, nextChapter.PreviousChapterId
        );
      }

      if (nextChapter != null)
      {
        isNextChapterUpdated = isNextChapterUpdated || UpdateFirstLink(originalChapter, nextChapter);

        if (isNextChapterUpdated)
        {
          await _chapterRepository.UpdateChapterAsync(nextChapter, onlyLinks: true);
        }
      }
    }

    internal bool UpdateFirstLink(Chapter targetChapter, Chapter sourceChapter)
    {
      targetChapter.FirstChapterId ??= sourceChapter.FirstChapterId;

      if (sourceChapter.FirstChapterId == null && targetChapter.FirstChapterId != null)
      {
        sourceChapter.FirstChapterId = targetChapter.FirstChapterId;
        return true;
      }

      if (targetChapter.FirstChapterId != sourceChapter.FirstChapterId)
      {
        _logger.LogWarning(
          "Chapter {} and {} have different first chapter links, despite being linked: {} != {}",
          targetChapter.Id, sourceChapter.Id, targetChapter.FirstChapterId, sourceChapter.FirstChapterId
        );
      }

      return false;
    }
  }
}
