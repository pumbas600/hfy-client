using System.ComponentModel.DataAnnotations;

namespace HfyClientApi.Models
{
  public class StoryMetadata
  {
    [Key]
    public string FirstChapterId { get; set; } = null!;

    public string CoverArtUrl { get; set; } = null!;
  }
}
