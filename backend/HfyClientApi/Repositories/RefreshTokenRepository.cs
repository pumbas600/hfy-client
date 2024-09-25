using HfyClientApi.Data;
using HfyClientApi.Models;

namespace HfyClientApi.Repositories
{

  public class RefreshTokenRepository : IRefreshTokenRepository
  {
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
    {
      return await _context.RefreshTokens.FindAsync(refreshToken);
    }

    public async Task<RefreshToken> SaveRefreshTokenAsync(RefreshToken refreshToken)
    {
      await _context.RefreshTokens.AddAsync(refreshToken);
      await _context.SaveChangesAsync();
      return refreshToken;
    }

    public async Task<RefreshToken> UpdateRefreshTokenAsync(RefreshToken refreshToken)
    {
      _context.RefreshTokens.Update(refreshToken);
      await _context.SaveChangesAsync();
      return refreshToken;
    }
  }
}
