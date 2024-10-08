using System.Threading.RateLimiting;

namespace HfyClientApi.Middleware
{
  public static class RateLimiterPolicies
  {
    public const string Public = "Public";
    public const string PublicLogin = "PublicLogin";
    public const string Authenticated = "Authenticated";
  }

  public static class RateLimiterSections
  {
    public const string Public = "RateLimiter:Public";
    public const string PublicLogin = "RateLimiter:PublicLogin";
    public const string Authenticated = "RateLimiter:Authenticated";
  }

  public class RateLimiterOptions
  {
    public int PermitLimit { get; set; }
    public int WindowSeconds { get; set; }
    public int SegmentsPerWindow { get; set; }
    public int QueueLimit { get; set; }
  }

  public class RateLimiter
  {
    public static void ConfigureRateLimiting(IConfiguration configuration, IServiceCollection services)
    {
      var publicOptions = configuration.GetSection(RateLimiterSections.Public)
        .Get<RateLimiterOptions>() ?? throw new ArgumentNullException(RateLimiterSections.Public);

      var publicLoginOptions = configuration.GetSection(RateLimiterSections.PublicLogin)
        .Get<RateLimiterOptions>() ?? throw new ArgumentNullException(RateLimiterSections.PublicLogin);

      var authenticatedOptions = configuration.GetSection(RateLimiterSections.Authenticated)
        .Get<RateLimiterOptions>() ?? throw new ArgumentNullException(RateLimiterSections.Authenticated);

      services.AddRateLimiter(limiterOptions =>
      {
        limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        limiterOptions.AddPolicy(RateLimiterPolicies.Public, partitioner: httpContext =>
        {
          var ipAddress = httpContext.Connection.RemoteIpAddress;
          return RateLimitPartition.GetSlidingWindowLimiter(ipAddress, _ =>
            new SlidingWindowRateLimiterOptions
            {
              PermitLimit = 50,
              Window = TimeSpan.FromSeconds(60),
              SegmentsPerWindow = 3,
              QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
              QueueLimit = 5,
            });
        });
      });
    }
  }
}
