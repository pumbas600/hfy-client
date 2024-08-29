using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using HfyClientApi.Extensions;
using HfyClientApi.Models;
using HtmlAgilityPack;
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
      public required string Label { get; set; }
      public required HtmlNode LinkElement { get; set; }

      public override string ToString()
        => $"ChapterLink(Subreddit: {Subreddit}, PostId: {PostId}, Label: {Label}, LinkElement: {LinkElement})";
    }

    private const string RedditBaseUrl = "https://www.reddit.com";

    /// <summary>
    /// A regex expression that allows the subreddit and post id to be extracted from a Reddit link.
    /// </summary>
    private const string RedditLinkRegex = @$"{RedditBaseUrl}/r/(\w+)/comments/(\w+)/\w+/?";

    public Chapter ChapterFromPost(SelfPost post)
    {
      var document = new HtmlDocument();
      document.LoadHtml(post.SelfTextHTML);

      var links = document.DocumentNode.SelectNodes("//a");

      Dictionary<string, List<ChapterLink>> nextLinkMap = [];
      Dictionary<string, List<ChapterLink>> previousLinkMap = [];

      if (links != null)
      {
        foreach (var linkElement in links)
        {
          var chapterLink = ParseRedditLink(linkElement);
          if (chapterLink == null)
          {
            continue;
          }

          if (chapterLink.Label.Contains("next"))
          {
            nextLinkMap.AddIfAbsent(chapterLink.PostId, []).Add(chapterLink);
          }
          else if (chapterLink.Label.Contains("prev"))
          {
            previousLinkMap.AddIfAbsent(chapterLink.PostId, []).Add(chapterLink);
          }
        }
      }

      // For now, we'll just use the first link found for next and previous...
      var nextLinks = nextLinkMap.Values.FirstOrDefault([]);
      var nextChapterId = nextLinks.FirstOrDefault()?.PostId;

      var previousLinks = previousLinkMap.Values.FirstOrDefault([]);
      var previousChapterId = previousLinks.FirstOrDefault()?.PostId;

      var linkElementsToRemove = nextLinks.Select(links => links.LinkElement)
        .Concat(previousLinks.Select(links => links.LinkElement));

      foreach (var linkElement in linkElementsToRemove)
      {
        linkElement.Remove();
      }

      // TODO: Determine link priority by checking if a corresponding link exists for prev/first links.
      // TODO: Cleanup any dangling '|' used to separate links.

      var chapter = new Chapter
      {
        Id = post.Id,
        Title = post.Title,
        TextHtml = document.DocumentNode.InnerHtml,
        IsNsfw = post.NSFW,
        CreatedAtUtc = post.Created,
        EditedAtUtc = post.Edited == default ? post.Created : post.Edited,
        ProcessedAtUtc = DateTime.UtcNow,
        NextChapterId = nextChapterId,
        PreviousChapterId = previousChapterId,
      };

      return chapter;
    }

    internal protected static ChapterLink? ParseRedditLink(HtmlNode linkElement)
    {
      var link = linkElement.GetAttributeValue("href", null);
      if (link == null || !link.StartsWith(RedditBaseUrl))
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

      string label = linkElement.InnerText;

      return new()
      {
        Subreddit = subreddit,
        PostId = postId,
        Label = label.ToLower(),
        LinkElement = linkElement
      };
    }


  }
}
