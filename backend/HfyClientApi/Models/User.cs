using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HfyClientApi.Models
{
  public class User : IdentityRole
  {
    [Key]
    public new string Name { get; set; } = null!;
    public string IconUrl { get; set; } = null!;
    public DateTime SyncedAt { get; set; }
    public WhitelistedUser? WhitelistedUser { get; set; }
  }
}
