using System.Diagnostics;
using HfyClientApi.Data;
using HfyClientApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HfyClientApi.Repositories
{
  public class HistoryRepository : IHistoryRepository
  {
    private readonly ILogger<HistoryRepository> _logger;
    private readonly AppDbContext _context;

    public HistoryRepository(ILogger<HistoryRepository> logger, AppDbContext context)
    {
      _logger = logger;
      _context = context;
    }

    public async Task<HistoryEntry> AddEntryAsync(HistoryEntry historyEntry)
    {
      await _context.AddAsync(historyEntry);
      await _context.SaveChangesAsync();
      return historyEntry;
    }

    public async Task<IEnumerable<HistoryEntry>> GetCurrentlyReadingChaptersAsync(string userName)
    {
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var currentlyReadingChapters = await _context.HistoryEntries
        .FromSql($"""
          SELECT * FROM (
            SELECT H.*, RANK() OVER (
                PARTITION BY C."FirstChapterId"
                ORDER BY H."ReadAtUtc"
            ) Rank
            FROM "HistoryEntries" H
            LEFT JOIN "Chapters" C ON C."Id" = H."ChapterId"
            WHERE H."UserName" = {userName}

          )
          WHERE Rank = 1
          ORDER BY "ReadAtUtc" DESC
        """)
        .Include(entry => entry.Chapter)
        .ToListAsync();

      // This generates the most horrendous SQL query ever... Idk how to manually adjust the SQL
      // var currentlyReadingChapters = await _context.HistoryEntries
      //   .Where(entry => entry.UserName == userName)
      //   .Include(entry => entry.Chapter)
      //   .GroupBy(chapter => chapter.Chapter.FirstChapterId)
      //   .Select(group => group.OrderByDescending(entry => entry.ReadAtUtc).First())
      //   .ToListAsync();

      stopwatch.Stop();
      _logger.LogInformation(
        "GetCurrentlyReadingChaptersAsync for user={} took {}ms",
        userName, stopwatch.ElapsedMilliseconds
      );

      return currentlyReadingChapters;
    }

    public async Task<HistoryEntry?> GetMostRecentEntryAsync(string userName)
    {
      return await _context.HistoryEntries
        .Where(entry => entry.UserName == userName)
        .OrderByDescending(entry => entry.ReadAtUtc)
        .FirstOrDefaultAsync();
    }
  }
}
