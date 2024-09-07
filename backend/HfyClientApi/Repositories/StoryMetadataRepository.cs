using HfyClientApi.Data;
using HfyClientApi.Exceptions;
using HfyClientApi.Models;
using HfyClientApi.Utils;

namespace HfyClientApi.Repositories
{
  public class StoryMetadataRepository : IStoryMetadataRepository
  {
    private const int MaxUpsertAttempts = 3;
    private readonly AppDbContext _context;
    private readonly ILogger<StoryMetadataRepository> _logger;

    public StoryMetadataRepository(AppDbContext context, ILogger<StoryMetadataRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task<StoryMetadata?> GetMetadataAsync(string firstChapterId)
    {
      return await _context.StoryMetadata.FindAsync(firstChapterId);
    }

    public async Task<Result> UpsertMetadataAsync(StoryMetadata metadata)
    {
      for (int attempt = 1; attempt <= MaxUpsertAttempts; attempt++)
      {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
          var storyMetadata = await GetMetadataAsync(metadata.FirstChapterId);
          if (storyMetadata != null)
          {
            storyMetadata.CoverArtUrl = metadata.CoverArtUrl;
          }
          else
          {
            await _context.StoryMetadata.AddAsync(metadata);
            await _context.SaveChangesAsync();
          }

          await transaction.CommitAsync();
        }
        catch (OperationCanceledException ex)
        {
          await transaction.RollbackAsync();
          _logger.LogError(
            ex, "Upsert story metadata id={} transaction cancelled, attempt={}/{}",
            metadata.FirstChapterId, attempt, MaxUpsertAttempts
          );
        }
      }

      return Errors.StoryMetadataUpsertFailed(metadata.FirstChapterId);
    }
  }
}
