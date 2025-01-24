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
          SELECT DISTINCT ON (C."FirstChapterId") H.*
          FROM "HistoryEntries" H
          LEFT JOIN "Chapters" C ON C."Id" = H."ChapterId"
          ORDER BY C."FirstChapterId", H."ReadAtUtc" DESC
        """)
        .Include(entry => entry.Chapter)
        .OrderByDescending(entry => entry.ReadAtUtc)
        .Select(entry => new HistoryEntry()
        {
          Id = entry.Id,
          ChapterId = entry.ChapterId,
          UserName = entry.UserName,
          ReadAtUtc = entry.ReadAtUtc,
          Chapter = new Chapter()
          { // Exclude the TextHtml column to reduce the payload size
            Id = entry.Chapter.Id,
            Author = entry.Chapter.Author,
            Subreddit = entry.Chapter.Subreddit,
            Title = entry.Chapter.Title,
            IsNsfw = entry.Chapter.IsNsfw,
            CreatedAtUtc = entry.Chapter.CreatedAtUtc,
            EditedAtUtc = entry.Chapter.EditedAtUtc,
            SyncedAtUtc = entry.Chapter.SyncedAtUtc,
            NextChapterId = entry.Chapter.NextChapterId,
            PreviousChapterId = entry.Chapter.PreviousChapterId,
            FirstChapterId = entry.Chapter.FirstChapterId,
            Downvotes = entry.Chapter.Downvotes,
            Upvotes = entry.Chapter.Upvotes,
            CoverArtUrl = entry.Chapter.CoverArtUrl,
          }
        })
        .ToListAsync();

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
