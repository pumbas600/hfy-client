namespace HfyClientApi.Dtos
{
  public class ReadingHistoryDto
  {
    public required ChapterMetadataDto ChapterMetadata { get; set; }
    public required DateTime ReadAtUtc { get; set; }
    public required string? NextChapterId { get; set; }
  }
}
