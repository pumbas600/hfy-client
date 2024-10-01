using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using HfyClientApi.Configuration;
using HfyClientApi.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace HfyClientApi.Services
{
  public class TokenService : ITokenService
  {
    private readonly JwtSettings _jwtSettings;

    public TokenService(JwtSettings jwtSettings)
    {
      _jwtSettings = jwtSettings;
    }

    public TokenDto GenerateAccessToken(string username)
    {
      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, username),
      };

      var token = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes),
        claims: claims,
        signingCredentials: new SigningCredentials(_jwtSettings.SigningKey, SecurityAlgorithms.HmacSha256)
      );

      return new TokenDto()
      {
        Value = new JwtSecurityTokenHandler().WriteToken(token),
        ExpiresAt = token.ValidTo,
      };
    }

    public TokenDto GenerateRefreshToken(string username)
    {
      var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
      var expiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

      return new TokenDto()
      {
        Value = token,
        ExpiresAt = expiresAt,
      };
    }
  }
}
