using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HfyClientApi.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HfyClientApi.Services {
  public class TokenService {
    private readonly JwtSettings _jwtSettings;

    public TokenService(JwtSettings jwtSettings) {
        _jwtSettings = jwtSettings;
    }

    public JwtSecurityToken GenerateAccessToken(List<Claim> claims) {
        return new JwtSecurityToken(
          issuer: _jwtSettings.Issuer,
          audience: _jwtSettings.Audience,
          expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes),
          claims: claims,
          signingCredentials: new SigningCredentials(_jwtSettings.SigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
  }
}
