using HfyClientApi.Dtos;
using HfyClientApi.Models;

namespace HfyClientApi.Services
{
  public interface IMapper
  {
    FullChapterDto ToFullChapterDto(Chapter chapter);

    ChapterMetadataDto ToChapterMetadataDto(Chapter chapter);

    ChapterPaginationDto ToPaginatedChapterMetadataDto(int pageSize, IEnumerable<Chapter> chapters);
  }
}
