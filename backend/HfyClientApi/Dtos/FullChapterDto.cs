namespace HfyClientApi.Dtos
{
  public class FullChapterDto : StoryDto
  {
    public required string ChapterId { get; set; }
    public required string Title { get; set; }
    public required string TextHtml { get; set; }
    public required bool IsNsfw { get; set; }

    public required DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// UTC time when the chapter was last edited. If it hasn't been edited, this will be the same
    /// as CreatedAtUtc.
    /// </summary>
    public required DateTime EditedAtUtc { get; set; }

    /// <summary>
    /// UTC time when the chapter was last processed by the system to determine the chapter links.
    /// </summary>
    public required DateTime ProcessedAtUtc { get; set; }

    /// <summary>
    /// The id of the next chapter in the story. If null, this is the latest chapter.
    /// </summary>
    public required string? NextChapterId { get; set; }

    /// <summary>
    /// The id of the previous chapter in the story. If null, this is the first chapter.
    /// </summary>
    public required string? PreviousChapterId { get; set; }
  }
}
