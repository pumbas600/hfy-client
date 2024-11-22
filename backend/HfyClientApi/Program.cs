using System.Text.Json.Serialization;
using HfyClientApi.BackgroundTasks;
using HfyClientApi.Configuration;
using HfyClientApi.Data;
using HfyClientApi.Middleware;
using HfyClientApi.Repositories;
using HfyClientApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Reddit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
  });

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
  ?? throw new ArgumentNullException("JwtSettings section in configuration is required");

var versionSettings = builder.Configuration.Get<VersionSettings>()
  ?? throw new ArgumentNullException($"{nameof(VersionSettings.ApiVersion)} key in configuration is required");

var reddit = new RedditClient(
  appId: builder.Configuration[Config.Keys.RedditAppId],
  appSecret: builder.Configuration[Config.Keys.RedditAppSecret],
  refreshToken: builder.Configuration[Config.Keys.RedditRefreshToken],
  accessToken: builder.Configuration[Config.Keys.RedditAccessToken],
  userAgent: versionSettings.UserAgent
);

// var hfySubreddit = reddit.Subreddit("HFY");
// var latestPost = hfySubreddit.Posts.New[0].About();
// Console.WriteLine(latestPost.Title);
// Console.WriteLine(latestPost.Author);
// Console.WriteLine(latestPost.Id);
// Console.WriteLine(latestPost.Created); // UTC Time
// Console.WriteLine(latestPost.Edited);
// Console.WriteLine(latestPost.UpVotes);
// Console.WriteLine(latestPost.DownVotes);
// Console.WriteLine(latestPost.Score);
// Console.WriteLine(latestPost.Permalink);
// Console.WriteLine(latestPost.NSFW);
// if (latestPost is SelfPost selfPost)
// {
//   // Console.WriteLine(currentFlair.FlairText);
//   // Console.WriteLine(currentFlair.FlairCssClass);
//   // Console.WriteLine(currentFlair.FlairTemplateId);
//   // Console.WriteLine(currentFlair.FlairPosition);
//   Console.WriteLine(selfPost.SelfTextHTML);
// }

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((config) =>
{
  config.UseInlineDefinitionsForEnums();
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
  options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton(reddit);
builder.Services.AddSingleton(versionSettings);
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<IChapterParsingService, ChapterParsingService>();
builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<IRedditService, RedditService>();
builder.Services.AddScoped<IRedditSynchronisationService, RedditSynchronisationService>();
builder.Services.AddScoped<ISubredditService, SubredditService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICipherService, CipherService>();

builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IStoryMetadataRepository, StoryMetadataRepository>();
builder.Services.AddScoped<ISubredditRepository, SubredditRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IHistoryEntriesRepository, HistoryEntriesRepository>();

builder.Services.AddHostedService<RedditSynchronisationBackgroundService>();

RateLimiter.ConfigureRateLimiting(builder.Configuration, builder.Services);

builder.Services.AddDataProtection();

builder.Services.AddHttpClient(
  Config.Clients.NoRedirect,
  client =>
  {
    client.DefaultRequestHeaders.UserAgent.ParseAdd(versionSettings.UserAgent);
  }
).ConfigurePrimaryHttpMessageHandler(() =>
{
  return new HttpClientHandler()
  {
    AllowAutoRedirect = false
  };
});

builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(policy =>
  {
    policy.WithOrigins(jwtSettings.Audience).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
  });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidIssuer = jwtSettings.Issuer,
      ValidAudience = jwtSettings.Audience,
      IssuerSigningKey = jwtSettings.SigningKey,
    };
    options.Events = new JwtBearerEvents
    {
      OnMessageReceived = context =>
      {
        // If there's already an authorization header, don't bother checking for cookies
        if (!context.Request.Headers.Authorization.IsNullOrEmpty())
        {
          return Task.CompletedTask;
        }

        if (context.Request.Cookies.TryGetValue(Config.Cookies.AccessToken, out var accessToken))
        {
          var cipherService = context.HttpContext.RequestServices.GetRequiredService<ICipherService>();
          var decryptedResult = cipherService.Decrypt(accessToken);
          if (decryptedResult.IsFailure)
          {
            context.Fail("Failed to decrypt access token cookie");
            return Task.CompletedTask;
          }

          context.Token = decryptedResult.Data;
        }

        return Task.CompletedTask;
      }
    };
  });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors(); // Enable cors!

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();
