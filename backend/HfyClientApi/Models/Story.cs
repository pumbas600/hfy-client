using System.ComponentModel.DataAnnotations;

namespace HfyClientApi.Models
{
  public class Story
  {

    [Key]
    public int Id { get; set; }
    public string Author { get; set; } = null!;
    public string Subreddit { get; set; } = null!;

    public ICollection<Chapter> Chapters { get; set; } = [];
    public string FirstChapterId { get; set; } = null!;
    public Chapter FirstChapter { get; set; } = null!;

  }
}
