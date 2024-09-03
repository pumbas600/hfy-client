using Newtonsoft.Json;

namespace HfyClientApi.Dtos.External
{
  public record AboutUserDto
  {
    [JsonProperty("icon_img")]
    public required string IconImg { get; set; }
  }
}
