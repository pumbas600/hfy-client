using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Data
{
  public class AppDbContext : DbContext
  {
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<StoryMetadata> StoryMetadata { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
  }
}
