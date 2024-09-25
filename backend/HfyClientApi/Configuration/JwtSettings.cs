using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HfyClientApi.Configuration {
  public class JwtSettings {
    public required string SecretKey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int AccessTokenExpirationInMinutes { get; set; }
    public required int RefreshTokenExpirationInDays { get; set; }

    public SymmetricSecurityKey SigningKey => new (Encoding.UTF8.GetBytes(SecretKey));
  }
}
