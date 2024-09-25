namespace HfyClientApi.Dtos
{
  public class UserDto
  {
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string IconUrl { get; set; }
  }

  public class TokenPairDto
  {
    public required TokenDto AccessToken { get; set; }
    public required TokenDto RefreshToken { get; set; }
  }

  public class LoginDto : TokenPairDto
  {
    public required UserDto User { get; set; }
  }

  public class RedditAccessTokenDto
  {
    public required string RedditAccessToken { get; set; }
  }
}
