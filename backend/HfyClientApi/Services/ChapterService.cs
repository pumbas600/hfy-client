using HfyClientApi.Dtos;
using HfyClientApi.Repositories;

namespace HfyClientApi.Services
{
  public class ChapterService : IChapterService
  {
    private readonly IChapterRepository _chapterRepository;
    private readonly IMapper _mapper;

    public ChapterService(IChapterRepository chapterRepository, IMapper mapper)
    {
      _chapterRepository = chapterRepository;
      _mapper = mapper;
    }

    public async Task<FullChapterDto> GetChapterByIdAsync(string id)
    {
      var chapter = await _chapterRepository.GetChapterByIdAsync(id) ?? throw new Exception($"Chapter with id {id} not found");
      return _mapper.ToFullChapterDto(chapter);
    }
  }
}
