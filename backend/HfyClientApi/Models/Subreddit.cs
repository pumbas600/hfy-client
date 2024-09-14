using System.ComponentModel.DataAnnotations;

namespace HfyClientApi.Models
{
  public class Subreddit
  {
    [Key]
    public string Name { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string IconUrl { get; set; } = null!;
    public string IconBackgroundColor { get; set; } = null!;
  }
}
