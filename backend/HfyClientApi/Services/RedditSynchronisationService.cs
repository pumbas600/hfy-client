
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
      await ProcessNewPostsAsync();

      using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));
      try
      {
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
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

      foreach (var post in _redditService.GetNewSelfPosts("HFY")) {
        await _chapterService.ProcessChapterByPostAsync(post);
      }
    }
  }
}
