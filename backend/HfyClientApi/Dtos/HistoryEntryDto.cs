namespace HfyClientApi.Dtos
{
  public class CreateHistoryEntryDto
  {
    public required string ChapterId { get; set; }
  }

  public class HistoryEntryDto : CreateHistoryEntryDto
  {
    public int Id { get; set; }
    public required string UserId { get; set; }
    public DateTime ReadAtUtc { get; set; }
  }
}
