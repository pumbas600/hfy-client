using HfyClientApi.Configuration;
using Reddit;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var reddit = new RedditClient(
  appId: builder.Configuration[Config.RedditAppId],
  appSecret: builder.Configuration[Config.RedditAppSecret],
  userAgent: Config.UserAgent
);

builder.Services.AddSingleton(reddit);

var hfySubreddit = reddit.Subreddit("HFY");
var topPost = hfySubreddit.Posts.Top[0].About();
Console.WriteLine(topPost.Title);
Console.WriteLine(topPost.Author);
Console.WriteLine(topPost.Id);
Console.WriteLine(topPost.ToString());


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
