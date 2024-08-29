using HfyClientApi.Models;
using HfyClientApi.Utils;

namespace HfyClientApi.Repositories
{
  public interface IChapterRepository
  {

    Task<Result<Chapter>> GetChapterByIdAsync(string id);

    Task<Result<Chapter>> UpsertChapterAsync(Chapter chapter);

    Task<Chapter> UpdateChapterAsync(Chapter chapter);

  }

}
