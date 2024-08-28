using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using HfyClientApi.Models;
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
      public required IElement LinkElement { get; set; }
    }

    private const string RedditBaseUrl = "https://www.reddit.com";

    /// <summary>
    /// A regex expression that allows the subreddit and post id to be extracted from a Reddit link.
    /// </summary>
    private const string RedditLinkRegex = @$"{RedditBaseUrl}/r/(\w+)/comments/(\w+)/\w+/?";

    private readonly HtmlParser parser;

    public ChapterParsingService()
    {
      parser = new HtmlParser();
    }

    public Chapter ChapterFromPost(SelfPost post)
    {
      var document = parser.ParseDocument(post.SelfTextHTML);
      var links = document.QuerySelectorAll("a");

      Dictionary<string, List<ChapterLink>> nextLinkMap = [];
      Dictionary<string, List<ChapterLink>> previousLinkMap = [];

      foreach (var linkElement in links)
      {
        var chapterLink = ParseRedditLink(linkElement);
        if (chapterLink == null)
        {
          continue;
        }

        if (chapterLink.Label.Contains("next"))
        {
          AddChapterLink(nextLinkMap, chapterLink);
        }
        else if (chapterLink.Label.Contains("prev"))
        {
          AddChapterLink(previousLinkMap, chapterLink);
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
        Subreddit = post.Subreddit,
        Title = post.Title,
        Author = post.Author,
        TextHTML = document.ToHtml(),
        Created = post.Created,
        Edited = post.Edited == default ? post.Created : post.Edited,
        NextChapterId = nextChapterId,
        PreviousChapterId = previousChapterId,
      };

      return chapter;
    }

    internal protected static void AddChapterLink(
      Dictionary<string, List<ChapterLink>> dict, ChapterLink chapterLink)
    {
      if (!dict.TryGetValue(chapterLink.PostId, out List<ChapterLink>? value))
      {
        value = ([]);
        dict[chapterLink.PostId] = value;
      }

      value.Add(chapterLink);
    }


    internal protected static ChapterLink? ParseRedditLink(IElement linkElement)
    {
      var link = linkElement.GetAttribute("href");
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

      string label = linkElement.TextContent;

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
