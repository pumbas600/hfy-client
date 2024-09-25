using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Models
{
  [Index(nameof(Token), IsUnique = true)]
  public class RefreshToken
  {
    [Key]
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public string Username { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
  }
}
