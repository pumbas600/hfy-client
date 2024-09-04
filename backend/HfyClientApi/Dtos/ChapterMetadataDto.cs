namespace HfyClientApi.Dtos
{
  public class ChapterMetadataDto
  {
    public required string Id { get; set; }
    public required string Author { get; set; }
    public required string Subreddit { get; set; }
    public required string Title { get; set; }
    public required bool IsNsfw { get; set; }
    public required int Upvotes { get; set; }
    public required int Downvotes { get; set; }
    public required string? CoverArtUrl { get; set; }

    /// <summary>
    /// The link to the author's Reddit profile.
    /// </summary>
    public required string RedditAuthorLink { get; set; }

    public required DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// UTC time when the chapter was last edited. If it hasn't been edited, this will be the same
    /// as CreatedAtUtc.
    /// </summary>
    public required DateTime EditedAtUtc { get; set; }

    /// <summary>
    /// UTC time when the chapter was last updated from the original post.
    /// </summary>
    public required DateTime SyncedAtUtc { get; set; }
  }
}
