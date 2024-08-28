using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface IChapterRepository
  {

    Task<Chapter> GetChapterByIdAsync(string id);
  }

}
