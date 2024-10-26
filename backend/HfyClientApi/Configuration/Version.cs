namespace HfyClientApi.Configuration
{
  public class VersionSettings
  {
    public required string ApiVersion { get; set; }
    public string UserAgent => $"hfy-client/{ApiVersion}";
  }
}
