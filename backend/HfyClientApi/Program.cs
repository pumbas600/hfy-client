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

// reddit.Models.OAuthCredentials.AccessToken

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

// Console.WriteLine(reddit.Models.OAuthCredentials.AccessToken);

// var httpClientHandler = new HttpClientHandler()
// {
//   AllowAutoRedirect = false
// };
// var client = new HttpClient(httpClientHandler);
// var request = new HttpRequestMessage(HttpMethod.Head, "https://oauth.reddit.com/r/HFY/s/V42RVh2fvi");
// request.Headers.Add("Authorization", "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IlNIQTI1NjpzS3dsMnlsV0VtMjVmcXhwTU40cWY4MXE2OWFFdWFyMnpLMUdhVGxjdWNZIiwidHlwIjoiSldUIn0.eyJzdWIiOiJ1c2VyIiwiZXhwIjoxNzI2Mzk4Mjk1Ljk3Mjk5NSwiaWF0IjoxNzI2MzExODk1Ljk3Mjk5NCwianRpIjoiYmZEeG9FbDRFdW1UUVhwb2lsZ095QTJlOWUzVWNnIiwiY2lkIjoiYWZCTWE2TDdVY1Z2VzlyVDA1ZjZkZyIsImxpZCI6InQyXzM2cDVuZGFsIiwiYWlkIjoidDJfMzZwNW5kYWwiLCJsY2EiOjE1NDk2ODg5NzcyMzgsInNjcCI6ImVKeEVqa0Z1eFRBSVJPX0NPamVxdXNBRzU2UGFJUUtjS3JldnFQUGIzV2dHWnQ0SFZHTWlDWWNOaGxMVkkwektETFhIR1NqOUwydXl3d1ktaTFlVHdxbkRabzFwVEI1MzUzeTZOREw1bGk5aGtzam4yMmY1M19GWnh2S1Z1dTVMbk9wdmlOWlJMT193NHVWb3ZQZ1h5QmdKTmpoTkxnd2U3STQ3ci1CVXkwNGhQa0xpaGcyNlhEend3RDFic0ZhZHg3TWFocTFKZlNpZjBqZXNVcHBMT2ZjR0c3ekVReTA3Rjl2blR3QUFBUF9fT1NCc2h3IiwicmNpZCI6IkQwNGhrMjlqU2ZQV0xmaUcya3lNVDVFQ3hwVE5KSkdIWlZhUmZ3UG4xNjgiLCJmbG8iOjh9.HF1DMTPCvGniRFSyRwHj0eDUHi43-ydgCZSU_Gs66amAJEaFe6d_BP3P3aWNvzrUIZJ11b-A-yyIqqzFGhaHuy4-FMwhE4oCqmuodMUaMn_kgWBsslGBulhfBpm4HFecnJvanz_yr5vMOgWrpE7THn4vSkS9by5no5jlO91bR1vr2fzokrJuwUDPbx1cYbvX1EIPSDM2qI7YtdQSB16rkEwYrqqmwI6p6i8OUNIKzNCFnmVVgEuMCXcrnCO_QC7U6oYXYfAE0e8U7On-Kwb1-BxRXtw7aYf5qyUe-nfVwQifd5RSHqvqnssNwiRf6gFmHwPLGrj17xYouCQLDnyn9Q");
// request.Headers.Add("User-Agent", "HfyClientApi/1.0");
// var response = await client.SendAsync(request);
// response.Headers.ToList().ForEach(header =>
// {
//   var value = string.Join(",", header.Value);
//   Console.WriteLine($"{header.Key}: {value}");
// });
// Console.WriteLine(response.StatusCode);
// var html = await response.Content.ReadAsStringAsync();
// Console.WriteLine(html);

// return;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
  options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton(reddit);
builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<IChapterParsingService, ChapterParsingService>();
builder.Services.AddScoped<IChapterService, ChapterService>();
builder.Services.AddScoped<IRedditService, RedditService>();
builder.Services.AddScoped<IRedditSynchronisationService, RedditSynchronisationService>();
builder.Services.AddScoped<ISubredditService, SubredditService>();

builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IStoryMetadataRepository, StoryMetadataRepository>();
builder.Services.AddScoped<ISubredditRepository, SubredditRepository>();

builder.Services.AddHostedService<RedditSynchronisationBackgroundService>();

builder.Services.AddHttpClient(
  Config.Clients.NoRedirect,
  client => {
    client.DefaultRequestHeaders.UserAgent.ParseAdd(Config.UserAgent);
  }
).ConfigurePrimaryHttpMessageHandler(() => {
  return new HttpClientHandler()
  {
    AllowAutoRedirect = false
  };
});

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
