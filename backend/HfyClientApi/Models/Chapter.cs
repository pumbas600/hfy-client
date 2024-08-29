using System.ComponentModel.DataAnnotations;

namespace HfyClientApi.Models
{
  public class Chapter
  {
    [Key]
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string TextHtml { get; set; } = null!;
    public bool IsNsfw { get; set; }
    public DateTime Created { get; set; }

    /// <summary>
    /// When the chapter was last edited. If it hasn't been edited, this will be the same as
    /// Created.
    /// </summary>
    public DateTime Edited { get; set; }

    /// <summary>
    /// The next chapter in the story. If null, this is the latest chapter.
    /// </summary>
    public string? NextChapterId { get; set; }

    /// <summary>
    /// The previous chapter in the story. If null, this is the first chapter.
    /// </summary>
    public string? PreviousChapterId { get; set; }

    public int StoryId { get; set; }
    public Story Story { get; set; } = null!;
  }
}
