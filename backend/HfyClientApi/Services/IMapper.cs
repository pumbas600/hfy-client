using HfyClientApi.Dtos;
using HfyClientApi.Models;

namespace HfyClientApi.Services
{
  public interface IMapper
  {
    public FullChapterDto ToFullChapterDto(Chapter chapter);
  }
}