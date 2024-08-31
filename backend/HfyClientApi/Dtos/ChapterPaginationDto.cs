namespace HfyClientApi.Dtos
{
  public class ChapterPaginationKey
  {
    public required DateTime LastCreatedAtUtc { get; set; }
    public required string LastPostId { get; set; }
  }

  public class ChapterPaginationDto : PaginationDto<ChapterPaginationKey, ChapterMetadataDto>
  {
  }
}
