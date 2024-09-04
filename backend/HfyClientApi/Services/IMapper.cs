using HfyClientApi.Dtos;
using HfyClientApi.Repositories;

namespace HfyClientApi.Services
{
  public interface IMapper
  {
    FullChapterDto ToFullChapterDto(CombinedChapter combinedChapter);

    ChapterMetadataDto ToChapterMetadataDto(CombinedChapter combinedChapter);

    ChapterPaginationDto ToPaginatedChapterMetadataDto(int pageSize, IEnumerable<CombinedChapter> combinedChapters);
  }
}
