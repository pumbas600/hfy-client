using HfyClientApi.Dtos;
using HfyClientApi.Models;
using HfyClientApi.Repositories;
using HfyClientApi.Utils;

namespace HfyClientApi.Services
{
  public class HistoryService : IHistoryService
  {
    private readonly IHistoryRepository _historyRepository;
    private readonly IMapper _mapper;

    public HistoryService(IHistoryRepository historyRepository, IMapper mapper)
    {
      _historyRepository = historyRepository;
      _mapper = mapper;
    }

    public async Task<Result<MaybeCreated<HistoryEntryDto>>> AddHistoryEntryAsync(
      string chapterId, string readerName)
    {

      var mostRecentHistoryEntry = await _historyRepository.GetMostRecentEntryAsync(readerName);

      if (mostRecentHistoryEntry != null && IsDuplicateEntry(mostRecentHistoryEntry, chapterId))
      {
        return new MaybeCreated<HistoryEntryDto>
        {
          IsCreated = false,
          Value = _mapper.ToHistoryEntryDto(mostRecentHistoryEntry),
        };
      }

      var newHistoryEntry = new HistoryEntry
      {
        ChapterId = chapterId,
        UserName = readerName,
        ReadAtUtc = DateTime.UtcNow,
      };

      var createdHistoryEntry = await _historyRepository.AddEntryAsync(newHistoryEntry);

      return new MaybeCreated<HistoryEntryDto>
      {
        IsCreated = true,
        Value = _mapper.ToHistoryEntryDto(createdHistoryEntry),
      };
    }

    public async Task<Result<IEnumerable<ChapterMetadataDto>>> GetCurrentlyReadingChaptersAsync(
      string userName)
    {
      var currentlyReadingChapters = await _historyRepository.GetCurrentlyReadingChaptersAsync(
        userName
      );

      return Result.Success(currentlyReadingChapters.Select(_mapper.ToChapterMetadataDto));
    }

    private static bool IsDuplicateEntry(HistoryEntry mostRecentHistoryEntry, string chapterId)
    {
      return mostRecentHistoryEntry.ChapterId == chapterId &&
        mostRecentHistoryEntry.ReadAtUtc.AddDays(1) > DateTime.UtcNow;
    }
  }
}
