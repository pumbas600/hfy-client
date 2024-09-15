namespace HfyClientApi.Dtos
{
  public class SubredditDto
  {
    public required string Name { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string IconUrl { get; set; }
    public required string IconBackgroundColor { get; set; }
    public required string RedditLink { get; set; }
  }
}
