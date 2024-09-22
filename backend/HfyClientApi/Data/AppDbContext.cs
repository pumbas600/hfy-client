using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Data
{
  public class AppDbContext : DbContext
  {
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<StoryMetadata> StoryMetadata { get; set; }
    public DbSet<Subreddit> Subreddits { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.HasPostgresExtension("fuzzystrmatch");
    }
  }
}
