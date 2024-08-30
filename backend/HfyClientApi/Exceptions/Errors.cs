using System.Net;
using HfyClientApi.Utils;

namespace HfyClientApi.Exceptions
{

  public static class Errors
  {
    public static class Codes
    {
      public const string PostNotFound = "Post.NotFound";
      public const string PostNotSelf = "Post.NotSelfPost";
      public const string ChapterNotFound = "Chapter.NotFound";
      public const string ChapterUpsertFailed = "Chapter.UpsertFailed";

    }

    public static Error PostNotFound(string postId) =>
      new(Codes.PostNotFound, $"No post found for id {postId}", HttpStatusCode.NotFound);
    public static Error PostNotSelfPost(string postId) =>
      new(Codes.PostNotSelf, $"The post with id {postId} is not a text post", HttpStatusCode.BadRequest);

    public static Error ChapterNotFound(string chapterId) =>
      new(Codes.ChapterNotFound, $"No chapter found for id {chapterId}", HttpStatusCode.NotFound);
    public static Error ChapterUpsertFailed(string chapterId) =>
      new(Codes.ChapterUpsertFailed, $"Failed to upsert chapter with id {chapterId}", HttpStatusCode.Conflict);
  }
}
