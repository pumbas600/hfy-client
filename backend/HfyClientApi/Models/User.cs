using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Models
{
  [Index(nameof(Name), IsUnique = true)]
  public class User : IdentityRole
  {
    public string IconUrl { get; set; } = null!;
    public DateTime SyncedAt { get; set; }
  }
}
