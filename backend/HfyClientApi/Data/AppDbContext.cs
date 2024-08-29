using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Data
{
  public class AppDbContext : DbContext
  {

    public DbSet<Story> Stories { get; set; }
    public DbSet<Chapter> Chapters { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
  }
}
