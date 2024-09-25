using HfyClientApi.Data;
using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;

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
      return await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken);
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
