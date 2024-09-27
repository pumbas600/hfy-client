using System.Text.Json.Serialization;
using HfyClientApi.BackgroundTasks;
using HfyClientApi.Configuration;
using HfyClientApi.Data;
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


var reddit = new RedditClient(
  appId: builder.Configuration[Config.Keys.RedditAppId],
  appSecret: builder.Configuration[Config.Keys.RedditAppSecret],
  refreshToken: builder.Configuration[Config.Keys.RedditRefreshToken],
  accessToken: builder.Configuration[Config.Keys.RedditAccessToken],
  userAgent: Config.UserAgent
);

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
  ?? throw new ArgumentNullException("JwtSettings section in configuration is required");

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
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<IChapterParsingService, ChapterParsingService>();
builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<IRedditService, RedditService>();
builder.Services.AddScoped<IRedditSynchronisationService, RedditSynchronisationService>();
builder.Services.AddScoped<ISubredditService, SubredditService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IStoryMetadataRepository, StoryMetadataRepository>();
builder.Services.AddScoped<ISubredditRepository, SubredditRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddHostedService<RedditSynchronisationBackgroundService>();

builder.Services.AddHttpClient(
  Config.Clients.NoRedirect,
  client =>
  {
    client.DefaultRequestHeaders.UserAgent.ParseAdd(Config.UserAgent);
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
    // TODO: Derive from  redirect url
    policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
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
        context.Token = context.Request.Cookies[Config.Cookies.AccessToken];
        return Task.CompletedTask;
      }
    };
  });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(); // Enable cors!

app.UseAuthorization();

app.MapControllers();

app.Run();
