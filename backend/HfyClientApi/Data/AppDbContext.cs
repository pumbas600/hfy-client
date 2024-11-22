using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Data
{
  public class AppDbContext : DbContext
  {
    public DbSet<Chapter> Chapters { get; set; } = null!;
    public DbSet<StoryMetadata> StoryMetadata { get; set; } = null!;
    public DbSet<Subreddit> Subreddits { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<WhitelistedUser> WhitelistedUsers { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<HistoryEntry> HistoryEntries { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.HasPostgresExtension("fuzzystrmatch");
      modelBuilder.Entity<User>()
        .HasOne(user => user.WhitelistedUser)
        .WithOne(whitelistedUser => whitelistedUser.User)
        .HasForeignKey<WhitelistedUser>(whitelistedUser => whitelistedUser.Name)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();

      modelBuilder.Entity<HistoryEntry>()
        .HasOne(historyEntry => historyEntry.User)
        .WithMany(user => user.HistoryEntries)
        .HasForeignKey(historyEntry => historyEntry.UserId)
        .OnDelete(DeleteBehavior.Cascade) // Delete all the history entries when the user is deleted
        .IsRequired();

      modelBuilder.Entity<HistoryEntry>()
        .HasOne(historyEntity => historyEntity.Chapter)
        .WithMany(chapter => chapter.HistoryEntries)
        .HasForeignKey(historyEntity => historyEntity.ChapterId)
        .OnDelete(DeleteBehavior.Cascade) // Delete all the history entries when the chapter is deleted
        .IsRequired();
    }
  }
}
