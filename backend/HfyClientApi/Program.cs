using HfyClientApi.BackgroundTasks;
using HfyClientApi.Configuration;
using HfyClientApi.Data;
using HfyClientApi.Repositories;
using HfyClientApi.Services;
using Microsoft.EntityFrameworkCore;
using Reddit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();


var reddit = new RedditClient(
  appId: builder.Configuration[Config.Keys.RedditAppId],
  appSecret: builder.Configuration[Config.Keys.RedditAppSecret],
  refreshToken: builder.Configuration[Config.Keys.RedditRefreshToken],
  accessToken: builder.Configuration[Config.Keys.RedditAccessToken],
  userAgent: Config.UserAgent
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
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton(reddit);
builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<IChapterParsingService, ChapterParsingService>();
builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<IRedditService, RedditService>();
builder.Services.AddScoped<IRedditSynchronisationService, RedditSynchronisationService>();

builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IStoryMetadataRepository, StoryMetadataRepository>();

// builder.Services.AddHostedService<RedditSynchronisationBackgroundService>();

builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(policy =>
  {
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
  });
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
