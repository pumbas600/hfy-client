using HfyClientApi.Models;
using HfyClientApi.Utils;

namespace HfyClientApi.Repositories
{
  public interface IStoryMetadataRepository
  {
    Task<StoryMetadata?> GetMetadataAsync(string firstChapterId);

    Task<Result> UpsertMetadataAsync(StoryMetadata metadata);
  }
}
