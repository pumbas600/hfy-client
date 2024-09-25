using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using HfyClientApi.Configuration;
using HfyClientApi.Dtos;
using HfyClientApi.Models;
using HfyClientApi.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace HfyClientApi.Services
{
  public class TokenService : ITokenService
  {
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtSettings _jwtSettings;

    public TokenService(IRefreshTokenRepository refreshTokenRepository, JwtSettings jwtSettings)
    {
      _refreshTokenRepository = refreshTokenRepository;
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

    public async Task<TokenDto> GenerateNewRefreshTokenAsync(string username)
    {
      var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
      var expiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

      var refreshToken = new RefreshToken()
      {
        Token = token,
        ExpiresAt = expiresAt,
        Username = username,
      };

      await _refreshTokenRepository.SaveRefreshTokenAsync(refreshToken);

      return new TokenDto()
      {
        Value = token,
        ExpiresAt = expiresAt,
      };
    }
  }
}
