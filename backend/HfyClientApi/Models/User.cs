using System.ComponentModel.DataAnnotations;

namespace HfyClientApi.Models
{
  public class User
  {
    [Key]
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string IconUrl { get; set; } = null!;
  }
}
