using System.Net;
using HfyClientApi.Utils;

namespace HfyClientApi.Exceptions
{

  public static class Errors
  {
    public static class Codes
    {
      public const string PostNotFound = "Post.NotFound";
      public const string ChapterNotFound = "Chapter.NotFound";
      public const string ChapterUpsertFailed = "Chapter.UpsertFailed";
      public const string StoryMetadataUpsertFailed = "StoryMetadata.UpsertFailed";
      public const string ChapterPaginationPartialKeyset = "Chapter.PaginationPartialKeyset";
      public const string UserNotFound = "User.NotFound";
    }

    public static Error PostNotFound(string postId) =>
      new(Codes.PostNotFound, $"No post found for id {postId}", HttpStatusCode.NotFound);

    public static Error ChapterNotFound(string chapterId) =>
      new(Codes.ChapterNotFound, $"No chapter found for id {chapterId}", HttpStatusCode.NotFound);
    public static Error ChapterUpsertFailed(string chapterId) =>
      new(Codes.ChapterUpsertFailed, $"Failed to upsert chapter with id {chapterId}", HttpStatusCode.Conflict);

    public static Error StoryMetadataUpsertFailed(string firstChapterId) =>
      new(Codes.StoryMetadataUpsertFailed, $"Failed to upsert story metadata with id {firstChapterId}", HttpStatusCode.Conflict);

    public static readonly Error ChapterPaginationPartialKeyset =
      new(Codes.ChapterPaginationPartialKeyset, $"Both lastCreated and lastId must be provided or omitted.", HttpStatusCode.BadRequest);

    public static Error UserNotFound(string username) =>
      new(Codes.UserNotFound, $"User {username} not found", HttpStatusCode.NotFound);
  }
}
