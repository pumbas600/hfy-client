using HfyClientApi.Services;

namespace HfyClientApi.BackgroundTasks
{
  public class RedditSynchronisationBackgroundService : BackgroundService
  {
    private readonly IServiceProvider  _serviceProvider;

    public RedditSynchronisationBackgroundService(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      using var scope = _serviceProvider.CreateScope();

      var synchronisationService = scope.ServiceProvider
        .GetRequiredService<IRedditSynchronisationService>();

      await synchronisationService.StartSynchronisationAsync(stoppingToken);
    }
  }
}
