using HfyClientApi.Dtos;

namespace HfyClientApi.Services
{
  public interface ITokenService
  {
    TokenDto GenerateAccessToken(string username);
    TokenDto GenerateRefreshToken(string username);
  }
}
