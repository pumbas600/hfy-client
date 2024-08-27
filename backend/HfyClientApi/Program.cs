using HfyClientApi.Configuration;
using Reddit;
using Reddit.Controllers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var reddit = new RedditClient(
  appId: builder.Configuration[Config.Keys.RedditAppId],
  appSecret: builder.Configuration[Config.Keys.RedditAppSecret],
  // refreshToken: builder.Configuration[Config.Keys.RedditRefreshToken],
  accessToken: builder.Configuration[Config.Keys.RedditAccessToken],
  userAgent: Config.UserAgent
);

builder.Services.AddSingleton(reddit);

var hfySubreddit = reddit.Subreddit("HFY");
var latestPost = hfySubreddit.Posts.New[0].About();
Console.WriteLine(latestPost.Title);
Console.WriteLine(latestPost.Author);
Console.WriteLine(latestPost.Id);
Console.WriteLine(latestPost.ToString());
Console.WriteLine(latestPost.Author);
Console.WriteLine(latestPost.Created); // UTC Time
Console.WriteLine(latestPost.Edited);
Console.WriteLine(latestPost.UpVotes);
Console.WriteLine(latestPost.DownVotes);
Console.WriteLine(latestPost.Score);
Console.WriteLine(latestPost.Permalink);
Console.WriteLine(latestPost);
if (latestPost is SelfPost selfPost)
{
  // Console.WriteLine(currentFlair.FlairText);
  // Console.WriteLine(currentFlair.FlairCssClass);
  // Console.WriteLine(currentFlair.FlairTemplateId);
  // Console.WriteLine(currentFlair.FlairPosition);
  Console.WriteLine(selfPost.SelfTextHTML);
}




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
