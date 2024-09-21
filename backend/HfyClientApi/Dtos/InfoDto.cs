namespace HfyClientApi.Dtos
{
  public enum BuildEnvironment
  {
    Development,
    Production
  }

  public class InfoDto
  {
    public required BuildEnvironment Environment { get; set; }
    public required string ApiVersion { get; set; }
  }
}
