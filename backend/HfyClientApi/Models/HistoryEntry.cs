using System.ComponentModel.DataAnnotations;

namespace HfyClientApi.Models
{
  public class HistoryEntry
  {
    [Key]
    public int Id { get; set; }
    public string ChapterId { get; set; } = null!;
    public Chapter Chapter { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    public DateTime ReadAtUtc { get; set; }
  }
}
