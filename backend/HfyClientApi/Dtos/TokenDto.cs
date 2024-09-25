namespace HfyClientApi.Dtos
{
  public class TokenDto
  {
    public required string Value { get; set; }
    public required DateTime ExpiresAt { get; set; }
  }

}
