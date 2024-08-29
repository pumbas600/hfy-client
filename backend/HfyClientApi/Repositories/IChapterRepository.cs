using HfyClientApi.Models;
using HfyClientApi.Utils;

namespace HfyClientApi.Repositories
{
  public interface IChapterRepository
  {

    Task<Result<Chapter>> GetChapterByIdAsync(string id);

    Task<Result<Chapter>> UpsertChapterAsync(Chapter chapter);

    Task<Chapter> UpdateChapterAsync(Chapter chapter);

    Task<Result<Chapter>> UpsertFirstChapterAsync(Story story, Chapter firstChapter);
  }

}
