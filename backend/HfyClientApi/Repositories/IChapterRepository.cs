using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface IChapterRepository
  {

    Task<Chapter?> GetChapterByIdAsync(string id);

    Task<Chapter> CreateChapterAsync(Chapter chapter);

    Task<Chapter> CreateFirstChapter(Story story, Chapter firstChapter);
  }

}
