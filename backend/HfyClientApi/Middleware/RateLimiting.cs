using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication;

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
              PermitLimit = publicOptions.PermitLimit,
              Window = TimeSpan.FromSeconds(publicOptions.WindowSeconds),
              SegmentsPerWindow = publicOptions.SegmentsPerWindow,
              QueueProcessingOrder = QueueProcessingOrder.NewestFirst,
              QueueLimit = publicOptions.QueueLimit,
            });
        });

        limiterOptions.AddPolicy(RateLimiterPolicies.PublicLogin, partitioner: httpContext =>
        {
          var ipAddress = httpContext.Connection.RemoteIpAddress;
          return RateLimitPartition.GetSlidingWindowLimiter(ipAddress, _ =>
            new SlidingWindowRateLimiterOptions
            {
              PermitLimit = publicLoginOptions.PermitLimit,
              Window = TimeSpan.FromSeconds(publicLoginOptions.WindowSeconds),
              SegmentsPerWindow = publicLoginOptions.SegmentsPerWindow,
              QueueProcessingOrder = QueueProcessingOrder.NewestFirst,
              QueueLimit = publicLoginOptions.QueueLimit,
            });
        });

        limiterOptions.AddPolicy(RateLimiterPolicies.Authenticated, partitioner: httpContext =>
        {
          var accessToken = httpContext.Features.Get<IAuthenticateResultFeature>()?
            .AuthenticateResult?.Properties?.GetTokenValue("access_token")?.ToString()
            ?? string.Empty;

          return RateLimitPartition.GetSlidingWindowLimiter(accessToken, _ =>
            new SlidingWindowRateLimiterOptions
            {
              PermitLimit = authenticatedOptions.PermitLimit,
              Window = TimeSpan.FromSeconds(authenticatedOptions.WindowSeconds),
              SegmentsPerWindow = authenticatedOptions.SegmentsPerWindow,
              QueueProcessingOrder = QueueProcessingOrder.NewestFirst,
              QueueLimit = authenticatedOptions.QueueLimit,
            });
        });
      });
    }
  }
}
