using System.Net;
using System.Threading.RateLimiting;

namespace HfyClientApi.Middleware
{
  public static class RateLimitingPolicies
  {
    public const string Public = "Public";
    public const string PublicLogin = "PublicLogin";
    public const string Authenticated = "Authenticated";
  }

  public class RateLimitingOptions
  {
    public int PermitLimit { get; set; }
    public int WindowSeconds { get; set; }
    public int SegmentsPerWindow { get; set; }
    public int QueueLimit { get; set; }
  }

  public class RateLimiting
  {
    public static void ConfigureRateLimiting(IServiceCollection services)
    {
      services.AddRateLimiter(limiterOptions =>
      {
        limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        limiterOptions.AddPolicy(RateLimitingPolicies.Public, partitioner: httpContext =>
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
