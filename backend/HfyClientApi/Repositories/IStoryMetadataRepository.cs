using HfyClientApi.Models;
using HfyClientApi.Utils;

namespace HfyClientApi.Repositories
{
  public interface IStoryMetadataRepository
  {
    Task<StoryMetadata?> GetMetadata(string firstChapterId);

    Task<Result> UpsertMetadata(StoryMetadata metadata);
  }
}
