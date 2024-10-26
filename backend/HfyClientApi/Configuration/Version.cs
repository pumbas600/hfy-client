namespace HfyClientApi.Configuration
{
  public class Version
  {
    public required string ApiVersion { get; set; }
    public string UserAgent => $"hfy-client/{ApiVersion}";
  }
}
