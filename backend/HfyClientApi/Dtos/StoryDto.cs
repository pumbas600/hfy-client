namespace HfyClientApi.Dtos
{
  public class StoryDto
  {
    public required int StoryId { get; set; }
    public required string Author { get; set; }
    public required string Subreddit { get; set; }
    public required string FirstChapterId { get; set; }
  }
}