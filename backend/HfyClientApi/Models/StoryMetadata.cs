using System.ComponentModel.DataAnnotations;

namespace HfyClientApi.Models
{
  public class StoryMetadata
  {
    [Key]
    public string FirstChapterId { get; set; } = null!;

    public string CoverImageUrl { get; set; } = null!;
  }
}
