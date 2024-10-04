using System.ComponentModel.DataAnnotations;

namespace HfyClientApi.Models
{
  public class WhitelistedUser
  {
    [Key]
    public string Name { get; set; } = null!;
    public User User { get; set; } = null!;

  }
}
