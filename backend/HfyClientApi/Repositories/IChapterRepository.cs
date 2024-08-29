using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface IChapterRepository
  {

    Task<Chapter?> GetChapterByIdAsync(string id);

    Task<Chapter> UpsertChapterAsync(Chapter chapter);

    Task<Chapter> UpdateChapterAsync(Chapter chapter);

    Task<Chapter> UpsertFirstChapter(Story story, Chapter firstChapter);
  }

}
