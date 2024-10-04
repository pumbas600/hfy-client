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
      public const string SubredditNotFound = "Subreddit.NotFound";
      public const string AuthSubjectMissing = "Auth.SubjectMissing";
      public const string AuthInvalidRedditAccessToken = "Auth.InvalidRedditAccessToken";
      public const string AuthInvalidRefreshToken = "Auth.InvalidRefreshToken";
      public const string AuthExpiredRefreshToken = "Auth.ExpiredRefreshToken";
      public const string AuthRefreshTokenMissing = "Auth.RefreshTokenMissing";
      public const string AuthCodeExchangeError = "Auth.CodeExchangeError";
      public const string DecryptMalformedCipherError = "Decrypt.MalformedCipherError";
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

    public static Error SubredditNotFound(string name) =>
      new(Codes.SubredditNotFound, $"Subreddit {name} not found", HttpStatusCode.NotFound);

    public static readonly Error AuthSubjectMissing =
      new(Codes.AuthSubjectMissing, "The subject claim is missing from the JWT", HttpStatusCode.Unauthorized);

    public static readonly Error AuthInvalidRedditAccessToken =
      new(Codes.AuthInvalidRedditAccessToken, "The Reddit access token is invalid", HttpStatusCode.Unauthorized);

    public static readonly Error AuthInvalidRefreshToken =
      new(Codes.AuthInvalidRefreshToken, "The refresh token is invalid", HttpStatusCode.Unauthorized);

    public static readonly Error AuthExpiredRefreshToken =
      new(Codes.AuthExpiredRefreshToken, "The refresh token has expired. Please login to get a new refresh token", HttpStatusCode.Unauthorized);

    public static readonly Error AuthRefreshTokenMissing =
      new(Codes.AuthRefreshTokenMissing, "Missing refresh token cookie", HttpStatusCode.Unauthorized);

    public static readonly Error AuthCodeExchangeError =
      new(Codes.AuthCodeExchangeError, "Failed to exchange the code for a Reddit access token", HttpStatusCode.Unauthorized);

    public static readonly Error DecryptMalformedCipherError =
      new(Codes.DecryptMalformedCipherError, $"Failed to decrypt text", HttpStatusCode.Unauthorized);
  }
}
