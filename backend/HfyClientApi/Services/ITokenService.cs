using HfyClientApi.Dtos;
using HfyClientApi.Migrations;

namespace HfyClientApi.Services
{
  public interface ITokenService
  {
    TokenDto GenerateAccessToken(string username);
    TokenDto GenerateRefreshToken(string username);
  }
}
