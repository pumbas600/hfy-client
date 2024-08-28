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
    public string? FirstChapterId { get; set; }

    /// <summary>
    /// This must be nullable because of the order in which entities are created. The story will
    /// be created first, resulting in a brief period where the first chapter is null.
    /// </summary>

    [ForeignKey(nameof(FirstChapterId))]
    public Chapter? FirstChapter { get; set; }

    [InverseProperty("Story")]
    public ICollection<Chapter> Chapters { get; } = [];

  }
}
