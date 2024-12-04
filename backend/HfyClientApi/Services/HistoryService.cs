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

    public async Task<Result<HistoryEntryDto>> AddHistoryEntryAsync(string id, string readerName)
    {
      var newHistoryEntry = new HistoryEntry
      {
        ChapterId = id,
        UserName = readerName,
        ReadAtUtc = DateTime.UtcNow,
      };

      var createdHistoryEntry = await _historyRepository.AddEntryAsync(newHistoryEntry);
      return _mapper.ToHistoryEntryDto(createdHistoryEntry);
    }

    public async Task<Result<IEnumerable<ChapterMetadataDto>>> GetCurrentlyReadingChaptersAsync(
      string userName)
    {
      var currentlyReadingChapters = await _historyRepository.GetCurrentlyReadingChaptersAsync(
        userName
      );

      return Result.Success(currentlyReadingChapters.Select(_mapper.ToChapterMetadataDto));
    }
  }
}
