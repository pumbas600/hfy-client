namespace HfyClientApi.Dtos
{
  public class UserDto
  {
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string IconUrl { get; set; }
  }

  public class LoginDto
  {
    public required string AccessToken { get; set; }
    public required UserDto User { get; set; }
  }
}
