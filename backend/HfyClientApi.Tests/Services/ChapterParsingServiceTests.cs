using AngleSharp.Html.Parser;
using HfyClientApi.Services;

namespace HfyClientApi.Tests.Services
{
  public class ChapterParsingServiceTests
  {
    private readonly HtmlParser htmlParser = new();

    [Fact]
    public void ParseRedditLink_WithValidChapterLink_IsParsed()
    {

      var link = "https://www.reddit.com/r/HFY/comments/vkksmy/humans_dont_hibernate/";
      var doc = htmlParser.ParseDocument($"<a href=\"{link}\">Next</a>");
      var linkElement = doc.QuerySelector("a")!;

      var parsedLink = ChapterParsingService.ParseRedditLink(linkElement);

      Assert.NotNull(parsedLink);
      Assert.Equal("HFY", parsedLink.Subreddit);
      Assert.Equal("vkksmy", parsedLink.PostId);
      Assert.Equal("next", parsedLink.Label);
    }

    [Theory]
    [InlineData("https://www.reddit.com/r/JCBWritingCorner/")]
    [InlineData("https://www.reddit.com/r/HFY/")]
    [InlineData("https://www.royalroad.com/fiction/70060/humans-dont-hibernate")]
    public void ParseRedditLink_WithInvalidChapterLink_ReturnsNull(string link)
    {
      var doc = htmlParser.ParseDocument($"<a href=\"{link}\">Next</a>");
      var linkElement = doc.QuerySelector("a")!;

      var parsedLink = ChapterParsingService.ParseRedditLink(linkElement);

      Assert.Null(parsedLink);
    }

  }
}
