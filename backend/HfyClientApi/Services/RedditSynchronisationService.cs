
using System.Diagnostics;

namespace HfyClientApi.Services {
  public class RedditSynchronisationService : IRedditSynchronisationService
  {
    private readonly IChapterService _chapterService;
    private readonly IRedditService _redditService;
    private readonly ILogger<RedditSynchronisationService> _logger;

    public RedditSynchronisationService(
      IChapterService chapterService, IRedditService redditService, ILogger<RedditSynchronisationService> logger)
    {
      _chapterService = chapterService;
      _redditService = redditService;
      _logger = logger;
    }

    public async Task StartSynchronisationAsync(CancellationToken stoppingToken)
    {
      var stopwatch = new Stopwatch();
      stopwatch.Start();
      await ProcessNewPostsAsync();

      using PeriodicTimer timer = new(TimeSpan.FromMinutes(2));

      try
      {
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
          stopwatch.Stop();
          _logger.LogInformation("Time since last synchronisation: {}", stopwatch.Elapsed);
          stopwatch.Restart();
          await ProcessNewPostsAsync();
        }
      }
      catch (OperationCanceledException)
      {
        _logger.LogInformation("Reddit synchronisation service is stopping");
      }
    }

    internal async Task ProcessNewPostsAsync() {
      _logger.LogInformation("Processing new posts in r/HFY");

      var newPosts = _redditService.GetNewSelfPosts("HFY");
      await _chapterService.ProcessChaptersByPostAsync(newPosts);

      _logger.LogInformation("Finished processing new posts in r/HFY");
    }
  }
}
