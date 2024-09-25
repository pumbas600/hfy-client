using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Models {

  [PrimaryKey(nameof(Username), nameof(Token))]
  public class RefreshToken {
    public string Username { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
  }
}
