using HfyClientApi.Exceptions;
using HfyClientApi.Utils;

namespace HfyClientApi.Dtos
{
  public class ChapterPaginationKey
  {
    public required DateTime LastCreatedAtUtc { get; set; }
    public required string LastPostId { get; set; }

    public static Result<ChapterPaginationKey?> From(DateTime? lastCreated, string? lastId)
    {
      if (lastCreated != null && lastId != null)
      {
        return new ChapterPaginationKey()
        {
          // For some reason, C# doesn't realise that lastCreated is not null.
          LastCreatedAtUtc = lastCreated.Value.ToUniversalTime(),
          LastPostId = lastId,
        };
      }

      if (lastCreated != null || lastId != null)
      {
        return Errors.ChapterPaginationPartialKeyset;
      }

      return Result.Success<ChapterPaginationKey?>(null);
    }
  }

  public class ChapterPaginationDto : PaginationDto<ChapterPaginationKey, ChapterMetadataDto>
  {
  }
}
