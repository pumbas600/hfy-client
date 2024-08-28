using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Data
{
  public class AppDbContext : DbContext
  {

    public DbSet<Story> Stories { get; set; }
    public DbSet<Chapter> Chapters { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Chapter>()
        .HasOne(e => e.PreviousChapter)
        .WithOne(e => e.NextChapter)
        .HasForeignKey<Chapter>(e => e.PreviousChapterId)
        .IsRequired(false);

      modelBuilder.Entity<Chapter>()
        .HasOne(e => e.NextChapter)
        .WithOne(e => e.PreviousChapter)
        .HasForeignKey<Chapter>(e => e.NextChapterId)
        .IsRequired(false);
    }
  }
}
