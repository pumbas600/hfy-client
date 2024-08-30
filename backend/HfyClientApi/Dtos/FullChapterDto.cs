namespace HfyClientApi.Dtos
{
  public class FullChapterDto
  {
    public required string Id { get; set; }
    public required string Author { get; set; }
    public required string Subreddit { get; set; }
    public required string Title { get; set; }
    public required string TextHtml { get; set; }
    public required bool IsNsfw { get; set; }

    /// <summary>
    /// The link to the Reddit post that this chapter was created from.
    /// </summary>
    public required string RedditPostLink { get; set; }

    public required DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// UTC time when the chapter was last edited. If it hasn't been edited, this will be the same
    /// as CreatedAtUtc.
    /// </summary>
    public required DateTime EditedAtUtc { get; set; }

    /// <summary>
    /// UTC time when the chapter was last updated from the original post.
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

    /// <summary>
    /// The id of the first chapter in the story. Chapters in the same story will all have the
    /// same FirstChapterId. If this is the first chapter, it will be the same as Id. If null, then
    /// the story this chapter belongs to has not been identified yet.
    /// </summary>
    public required string? FirstChapterId { get; set; }
  }
}
