using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{
  public interface IRefreshTokenRepository
  {
    Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);

    Task<RefreshToken> SaveRefreshTokenAsync(RefreshToken refreshToken);

    Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken refreshToken);

    Task DeleteRefreshTokenAsync(string refreshToken);
  }
}
