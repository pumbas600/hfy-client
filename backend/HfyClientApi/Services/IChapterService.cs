using HfyClientApi.Dtos;

namespace HfyClientApi.Services
{
  public interface IChapterService
  {
    Task<FullChapterDto> GetChapterByIdAsync(string id);
  }
}
