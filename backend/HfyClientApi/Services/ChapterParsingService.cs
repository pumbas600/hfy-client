using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using HfyClientApi.Configuration;
using HfyClientApi.Extensions;
using HfyClientApi.Models;
using HtmlAgilityPack;
using OpenGraphNet;
using Reddit;
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
    private const string RedditLinkRegex = @$"/r/(\w+)/comments/(\w+)/\w+/?";

    private readonly RedditClient _redditClient;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<ChapterParsingService> _logger;
    private readonly VersionSettings _versionSettings;

    public ChapterParsingService(
      RedditClient redditClient, IHttpClientFactory clientFactory,
      ILogger<ChapterParsingService> logger, VersionSettings versionSettings)
    {
      _redditClient = redditClient;
      _clientFactory = clientFactory;
      _logger = logger;
      _versionSettings = versionSettings;
    }

    public DateTime GetEditedAtUtc(SelfPost post)
    {
      return post.Edited == default ? post.Created : post.Edited;
    }

    public async Task<(Chapter, StoryMetadata?)> ChapterFromPostAsync(SelfPost post)
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

          if (!IsRedditLink(link))
          {
            if (coverArtUrl != null)
            {
              continue;
            }

            if (label.Contains("cover"))
            {
              link = RemoveQueryParameters(link);
              if (link != null && await IsImageUrlAsync(link))
              {
                coverArtUrl = link;
              }
            }
            else if (link.StartsWith("https://www.royalroad.com/fiction"))
            {
              coverArtUrl = await GetCoverArtUrlFromRoyalRoadLink(link);
            }
          }
          else
          {
            var chapterLink = await ParseRedditLink(linkElement);
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
      // TODO: Cleanup any dangling '|', or '[', ']' used to separate links.

      if (firstChapterId == null && previousChapterId == null)
      {
        firstChapterId = post.Id;
      }

      StoryMetadata? storyMetadata = null;
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
        EditedAtUtc = GetEditedAtUtc(post),
        SyncedAtUtc = DateTime.UtcNow,
        NextChapterId = nextChapterId,
        PreviousChapterId = previousChapterId,
        FirstChapterId = firstChapterId,
      };

      return (chapter, storyMetadata);
    }

    internal bool IsRedditLink(string link)
    {
      return Config.SupportedRedditUrls.Any(link.StartsWith);
    }

    internal async protected Task<ChapterLink?> ParseRedditLink(HtmlNode linkElement)
    {
      var link = linkElement.GetAttributeValue("href", null);

      // Handle share links from mobile /s/
      if (link != null && link.Contains("/s/"))
      {
        link = await GetShareLinkLocationAsync(link);
      }

      if (link == null)
      {
        return null;
      }

      foreach (var redditLinkBase in Config.SupportedRedditUrls)
      {
        link = link.Replace(redditLinkBase, "");
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

    internal protected async Task<bool> IsImageUrlAsync(string url)
    {
      List<string> imageFiletypes = [".jpg", ".jpeg", ".gif", ".png", ".bmp", ".webp"];

      if (!imageFiletypes.Any(url.EndsWith))
      {
        return false;
      }

      using HttpClient client = _clientFactory.CreateClient();

      var request = new HttpRequestMessage(HttpMethod.Head, url);
      request.Headers.Add("User-Agent", _versionSettings.UserAgent);

      var response = await client.SendAsync(request);
      var mediaType = response.Content.Headers.ContentType?.MediaType;

      // SVGs can include embedded JS so we want to avoid those for security reasons.
      var isImageMediaType = response.IsSuccessStatusCode && mediaType != null
        && mediaType.StartsWith("image/") && !mediaType.Contains("svg");

      return isImageMediaType;
    }

    internal protected async Task<string?> GetCoverArtUrlFromRoyalRoadLink(string royalRoadLink)
    {
      _logger.LogInformation("Fetching cover art from Royal Road link: {} ", royalRoadLink);
      try
      {
        var graph = await OpenGraph.ParseUrlAsync(royalRoadLink);
        var imageUrl = RemoveQueryParameters(graph.Image?.ToString());

        // Default Royal Road cover art. Sometimes this can start with file:///
        if (imageUrl == null || imageUrl.EndsWith("/dist/img/nocover-new-min.png")
          || !await IsImageUrlAsync(imageUrl))
        {
          return null;
        }

        return imageUrl;
      }
      catch (HttpRequestException e)
      {
        _logger.LogError(e, "Failed to fetch cover art from Royal Road link: {}", royalRoadLink);
        return null;
      }
    }

    internal static string? RemoveQueryParameters(string? url)
    {
      if (url == null)
      {
        return null;
      }

      return url.Split("?")[0];
    }

    public async Task<string?> GetShareLinkLocationAsync(string shareLink)
    {
      using HttpClient client = _clientFactory.CreateClient(Config.Clients.NoRedirect);

      try
      {
        // We need to make requests to the OAuth Reddit URL
        shareLink = shareLink.Replace(Config.RedditUrl, Config.OauthRedditUrl);

        // It seems like Reddit doesn't ratelimit this thankfully :)
        var request = new HttpRequestMessage(HttpMethod.Head, shareLink);
        var accessToken = _redditClient.Models.OAuthCredentials.AccessToken;
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        var response = await client.SendAsync(request);
        // Just to be safe, check to ensure we're not being ratelimited for this request
        if (response.Headers.Contains("x-ratelimit-used"))
        {
          _logger.LogCritical("Reddit ratelimit used parsing share link!");
        }

        if (response.StatusCode == HttpStatusCode.MovedPermanently
            || response.StatusCode == HttpStatusCode.Redirect)
        {

          var shareLinkLocation = response.Headers.Location?.ToString();
          if (shareLinkLocation != null)
          {
            _logger.LogInformation("Resolved share link {} to {}", shareLink, shareLinkLocation);
          }
          return shareLinkLocation;
        }
      }
      catch (HttpRequestException e)
      {
        _logger.LogError(e, "Failed to fetch share link location header: {}", shareLink);
      }

      return null;
    }
  }
}
