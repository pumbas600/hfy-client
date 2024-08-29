using System.Net;
using HfyClientApi.Utils;

namespace HfyClientApi.Exceptions
{
  public static class Errors
  {
    public static Error PostNotFound(string postId) =>
      new("Posts.NotFound", $"No post found for id {postId}", HttpStatusCode.NotFound);
    public static Error PostNotSelfPost(string postId) =>
      new("Posts.NotSelfPost", $"The post with id {postId} is not a text post", HttpStatusCode.BadRequest);

    public static Error ChapterNotFound(string chapterId) =>
      new("Chapters.NotFound", $"No chapter found for id {chapterId}", HttpStatusCode.NotFound);
  }
}
