namespace HfyClientApi.Services {
  public interface IRedditSynchronisationService {
    Task StartSynchronisationAsync(CancellationToken stoppingToken);
  }
}
