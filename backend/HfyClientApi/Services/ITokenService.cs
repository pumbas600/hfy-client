using HfyClientApi.Dtos;

namespace HfyClientApi.Services
{
  public interface ITokenService
  {
    TokenDto GenerateAccessToken(string username);
    Task<TokenDto> GenerateNewRefreshTokenAsync(string username);
  }
}
