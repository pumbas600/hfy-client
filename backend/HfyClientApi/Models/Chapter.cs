namespace HfyClientApi.Models
{
  public class Chapter
  {
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string TextHTML { get; set; } = null!;
    public DateTime Created { get; set; }

    /// <summary>
    /// When the chapter was last edited. If it hasn't been edited, this will be the same as
    /// Created.
    /// </summary>
    public DateTime Edited { get; set; }

    /// <summary>
    /// The next chapter in the series. If null, this is the latest chapter.
    /// </summary>
    public string? NextChapterId { get; set; }

    /// <summary>
    /// The previous chapter in the series. If null, this is the first chapter.
    /// </summary>
    public string? PreviousChapterId { get; set; }

    /// <summary>
    /// The first chapter in the series. If this is the first chapter, it will be its own id.
    /// </summary>
    public string FirstChapterId { get; set; } = null!;
  }
}
