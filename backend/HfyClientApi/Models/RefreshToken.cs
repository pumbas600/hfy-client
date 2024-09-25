using System.ComponentModel.DataAnnotations;

namespace HfyClientApi.Models
{
  public class RefreshToken
  {
    [Key]
    public string Token { get; set; } = null!;
    public string Username { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
  }
}
