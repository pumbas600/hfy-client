using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using HfyClientApi.Configuration;
using HfyClientApi.Extensions;
using HfyClientApi.Models;
using HtmlAgilityPack;
using OpenGraphNet;
using Reddit.Controllers;

[assembly: InternalsVisibleTo("HfyClientApi.Tests")]

namespace HfyClientApi.Services
{

  public class ChapterParsingService : IChapterParsingService
  {

    internal protected class ChapterLink
    {
      public required string Subreddit { get; set; }
      public required string PostId { get; set; }
      public required HtmlNode LinkElement { get; set; }

      public override string ToString()
        => $"ChapterLink(Subreddit: {Subreddit}, PostId: {PostId}, LinkElement: {LinkElement})";
    }

    /// <summary>
    /// A regex expression that allows the subreddit and post id to be extracted from a Reddit link.
    /// </summary>
    private const string RedditLinkRegex = @$"{Config.RedditUrl}/r/(\w+)/comments/(\w+)/\w+/?";

    private readonly ILogger<ChapterParsingService> _logger;

    public ChapterParsingService(ILogger<ChapterParsingService> logger)
    {
      _logger = logger;
    }

    public async Task<(Chapter, StoryMetadata?)> ChapterFromPost(SelfPost post)
    {
      var document = new HtmlDocument();
      document.LoadHtml(post.SelfTextHTML);

      var links = document.DocumentNode.SelectNodes("//a");

      Dictionary<string, List<ChapterLink>> nextLinkMap = [];
      Dictionary<string, List<ChapterLink>> previousLinkMap = [];
      Dictionary<string, List<ChapterLink>> firstLinkMap = [];

      string? coverArtUrl = null;

      if (links != null)
      {
        foreach (var linkElement in links)
        {
          var label = linkElement.InnerText.ToLower();
          var link = linkElement.GetAttributeValue("href", null);
          if (link == null)
          {
            continue;
          }

          if (!link.StartsWith(Config.RedditUrl))
          {
            if (label.Contains("cover") && IsImageUrl(link))
            {
              coverArtUrl = link;
            }
            else if (coverArtUrl == null && link.StartsWith("https://www.royalroad.com/fiction"))
            {
              coverArtUrl = await GetCoverArtUrlFromRoyalRoadLink(link);
            }
          }
          else
          {
            var chapterLink = ParseRedditLink(linkElement);
            if (chapterLink == null)
            {
              continue;
            }

            if (label.Contains("next"))
            {
              nextLinkMap.AddIfAbsent(chapterLink.PostId, []).Add(chapterLink);
            }
            else if (label.Contains("prev"))
            {
              previousLinkMap.AddIfAbsent(chapterLink.PostId, []).Add(chapterLink);
            }
            else if (label.Contains("first"))
            {
              firstLinkMap.AddIfAbsent(chapterLink.PostId, []).Add(chapterLink);
            }
            else
            {
              // Update the link to point to not go to Reddit
              chapterLink.LinkElement.SetAttributeValue("href", $"/chapters/{chapterLink.PostId}");
            }
          }
        }
      }

      // For now, we'll just use the first link found for next and previous...
      var nextLinks = nextLinkMap.Values.FirstOrDefault([]);
      var nextChapterId = nextLinks.FirstOrDefault()?.PostId;

      var previousLinks = previousLinkMap.Values.FirstOrDefault([]);
      var previousChapterId = previousLinks.FirstOrDefault()?.PostId;

      var firstLinks = firstLinkMap.Values.FirstOrDefault([]);
      var firstChapterId = firstLinks.FirstOrDefault()?.PostId;

      var linkElementsToRemove = nextLinks.Select(links => links.LinkElement)
        .Concat(previousLinks.Select(links => links.LinkElement))
        .Concat(firstLinks.Select(links => links.LinkElement));

      foreach (var linkElement in linkElementsToRemove)
      {
        linkElement.Remove();
      }

      // TODO: Determine link priority by checking if a corresponding link exists for prev/first links.
      // TODO: Cleanup any dangling '|' used to separate links.

      if (firstChapterId == null && previousChapterId == null)
      {
        firstChapterId = post.Id;
      }

      StoryMetadata? storyMetadata = null;
      Console.WriteLine("Cover art URL: " + coverArtUrl);

      if (firstChapterId != null && coverArtUrl != null)
      {
        storyMetadata = new()
        {
          FirstChapterId = firstChapterId,
          CoverArtUrl = coverArtUrl
        };
      }

      var chapter = new Chapter()
      {
        Id = post.Id,
        Title = post.Title,
        Author = post.Author,
        Subreddit = post.Subreddit,
        Upvotes = post.UpVotes,
        Downvotes = post.DownVotes,
        TextHtml = document.DocumentNode.InnerHtml,
        IsNsfw = post.NSFW,
        CreatedAtUtc = post.Created,
        EditedAtUtc = post.Edited == default ? post.Created : post.Edited,
        SyncedAtUtc = DateTime.UtcNow,
        NextChapterId = nextChapterId,
        PreviousChapterId = previousChapterId,
        FirstChapterId = firstChapterId,
      };

      return (chapter, storyMetadata);
    }

    internal protected static ChapterLink? ParseRedditLink(HtmlNode linkElement)
    {
      var link = linkElement.GetAttributeValue("href", null);
      if (link == null)
      {
        return null;
      }

      var match = Regex.Match(link, RedditLinkRegex, RegexOptions.IgnoreCase);
      if (!match.Success || match.Groups.Count < 3)
      {
        return null;
      }

      string subreddit = match.Groups[1].Value;
      string postId = match.Groups[2].Value;

      return new()
      {
        Subreddit = subreddit,
        PostId = postId,
        LinkElement = linkElement
      };
    }

    internal protected static bool IsImageUrl(string url)
    {
      List<string> imageFiletypes = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp"];

      if (!imageFiletypes.Any(url.EndsWith))
      {
        return false;
      }

      // TODO: Make a HEAD request and check the Content-Type header
      return true;
    }

    internal protected async Task<string?> GetCoverArtUrlFromRoyalRoadLink(
      string royalRoadLink)
    {
      _logger.LogInformation("Fetching cover art from Royal Road link: {}", royalRoadLink);
      var graph = await OpenGraph.ParseUrlAsync(royalRoadLink);
      return graph.Image?.ToString();

    }
  }
}
