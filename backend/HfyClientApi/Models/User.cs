using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Models
{
  [Index(nameof(Username), IsUnique = true)]
  public class User
  {
    [Key]
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string IconUrl { get; set; } = null!;
  }
}
