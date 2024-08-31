using HfyClientApi.Dtos;
using HfyClientApi.Models;

namespace HfyClientApi.Services
{
  public interface IMapper
  {
    public FullChapterDto ToFullChapterDto(Chapter chapter);

    public ChapterMetadataDto ToChapterMetadataDto(Chapter chapter);

    public ChapterPaginationDto ToPaginatedChapterMetadataDto(
      int pageSize, IEnumerable<Chapter> chapters);
  }
}
