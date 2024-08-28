using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HfyClientApi.Models
{
  public class Story
  {

    [Key]
    public int Id { get; set; }
    public string Author { get; set; } = null!;
    public string Subreddit { get; set; } = null!;
    public string FirstChapterId { get; set; } = null!;

    [ForeignKey(nameof(FirstChapterId))]
    public Chapter FirstChapter { get; set; } = null!;

    [InverseProperty("Story")]
    public ICollection<Chapter> Chapters { get; } = [];

  }
}
