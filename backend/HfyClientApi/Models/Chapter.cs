using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Models
{
  [Index(nameof(Subreddit))]
  [Index(nameof(CreatedAtUtc))]
  [Index(nameof(FirstChapterId))]
  public class Chapter
  {
    [Key]
    public string Id { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Subreddit { get; set; } = null!;
    public string Title { get; set; } = null!;

    /// <summary>
    /// A lowercase, special character stripped version of the title used for searching.
    /// </summary>
    public string SearchableTitle { get; set; } = null!;
    public int Upvotes { get; set; }
    public int Downvotes { get; set; }
    public string TextHtml { get; set; } = null!;
    public bool IsNsfw { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// UTC time when the chapter was last edited. If it hasn't been edited, this will be the same
    // as CreatedAtUtc.
    /// </summary>
    public DateTime EditedAtUtc { get; set; }

    /// <summary>
    /// UTC time when the chapter was last updated from the original post.
    /// </summary>
    public DateTime SyncedAtUtc { get; set; }

    /// <summary>
    /// The next chapter in the story. If null, this is the latest chapter.
    /// </summary>
    public string? NextChapterId { get; set; }

    /// <summary>
    /// The previous chapter in the story. If null, this is the first chapter.
    /// </summary>
    public string? PreviousChapterId { get; set; }

    /// <summary>
    /// The id of the first chapter in the story. Chapters in the same story will all have the
    /// same FirstChapterId. If this is the first chapter, it will be the same as Id. If null, then
    /// the story this chapter belongs to has not been identified yet.
    /// </summary>
    public string? FirstChapterId { get; set; } = null!;

    public List<HistoryEntry> HistoryEntries { get; set; } = null!;
  }
}
