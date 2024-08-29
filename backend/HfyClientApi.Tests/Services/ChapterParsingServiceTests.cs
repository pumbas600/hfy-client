using HfyClientApi.Services;
using HtmlAgilityPack;
using Reddit.Controllers;

namespace HfyClientApi.Tests.Services
{
  public class ChapterParsingServiceTests
  {
    private readonly ChapterParsingService chapterParsingService = new();

    [Fact]
    public void ParseRedditLink_WithValidChapterLink_IsParsed()
    {

      var link = "https://www.reddit.com/r/HFY/comments/sdffas/my_example_story/";
      var linkElement = HtmlNode.CreateNode($"<a href=\"{link}\">Next</a>");

      var parsedLink = ChapterParsingService.ParseRedditLink(linkElement);

      Assert.NotNull(parsedLink);
      Assert.Equal("HFY", parsedLink.Subreddit);
      Assert.Equal("sdffas", parsedLink.PostId);
      Assert.Equal("next", parsedLink.Label);
    }

    [Theory]
    [InlineData("https://www.reddit.com/r/MySubreddit/")]
    [InlineData("https://www.reddit.com/r/HFY/")]
    [InlineData("https://www.royalroad.com/fiction/70060/my_example_story")]
    public void ParseRedditLink_WithInvalidChapterLink_ReturnsNull(string link)
    {
      var linkElement = HtmlNode.CreateNode($"<a href=\"{link}\">Next</a>");
      var parsedLink = ChapterParsingService.ParseRedditLink(linkElement);

      Assert.Null(parsedLink);
    }

    [Theory]
    [InlineData("Prev")]
    [InlineData("Previous")]
    [InlineData("prev")]
    [InlineData("previous")]
    [InlineData("< Prev")]
    [InlineData("< Previous")]
    [InlineData("< prev")]
    [InlineData("Previous Chapter")]
    [InlineData("⏮Previous")]
    [InlineData("⏮ Previous")]
    [InlineData("⏮Prev")]
    public void ChapterFromPost_WithOnlyPreviousLink_ExtractsPreviousLink(string previousLinkLabel)
    {
      var textHtml = BuildPostHtml(
        $"""
        <p>
          <a href="https://www.reddit.com/r/HFY/comments/sdffas/my_example_story/">Random Link</a> |
          <a href="https://www.reddit.com/r/HFY/comments/1exzyx5/my_example_story_100/">{previousLinkLabel}</a> |
          Next
        </p>
        """,
        """
        <p>
          <a href="https://www.reddit.com/r/HFY/comments/sdffas/my_example_story/">Random Link</a> |
          <a href="https://www.reddit.com/r/HFY/comments/1exzyx5/my_example_story_100/">Previous</a> |
          Next
        </p>
        """
      );

      SelfPost post = new(null, "HFY", "My Example Story", "pumbas600", null, textHtml, "sdfghj");

      var chapter = chapterParsingService.ChapterFromPost(post);

      Assert.Equal("My Example Story", chapter.Title);
      Assert.Equal("sdfghj", chapter.Id);
      Assert.Equal(BuildPostHtml(
        """
        <p>
          <a href="https://www.reddit.com/r/HFY/comments/sdffas/my_example_story/">Random Link</a> |
           |
          Next
        </p>
        """,
        """
        <p>
          <a href="https://www.reddit.com/r/HFY/comments/sdffas/my_example_story/">Random Link</a> |
           |
          Next
        </p>
        """), chapter.TextHtml);
      Assert.Equal("1exzyx5", chapter.PreviousChapterId);
      Assert.Null(chapter.NextChapterId);
      Assert.Null(chapter.FirstChapterId);
    }

    [Theory]
    [InlineData("Next")]
    [InlineData("next")]
    [InlineData("Next >")]
    [InlineData("next >")]
    [InlineData("Next Chapter")]
    [InlineData("Next⏭")]
    [InlineData("Next ⏭")]
    public void ChapterFromPost_WithOnlyNextLink_ExtractsNextLink(string nextLinkLabel)
    {
      var textHtml = BuildPostHtml(
        $"""
        <p><a href="https://www.reddit.com/r/HFY/comments/1exzyx5/my_example_story_100/">{nextLinkLabel}</a></p>
        """,
        """
        <p><a href="https://www.reddit.com/r/HFY/comments/1exzyx5/my_example_story_100/">Next</a></p>
        """
      );

      SelfPost post = new(null, "HFY", "My Example Story", "pumbas600", null, textHtml, "sdfghj");

      var chapter = chapterParsingService.ChapterFromPost(post);

      Assert.Equal("My Example Story", chapter.Title);
      Assert.Equal("sdfghj", chapter.Id);
      Assert.Equal(BuildPostHtml("<p></p>", "<p></p>"), chapter.TextHtml);
      Assert.Equal("1exzyx5", chapter.NextChapterId);
      Assert.Null(chapter.PreviousChapterId);
      Assert.Null(chapter.FirstChapterId);
    }

    [Theory]
    [InlineData("First")]
    [InlineData("first")]
    [InlineData("<< First")]
    [InlineData("<< first")]
    [InlineData("First chapter")]
    [InlineData("⏮First")]
    [InlineData("⏮ First")]
    public void ChapterFromPost_WithOnlyFirstLink_ExtractsFirstLink(string firstLinkLabel)
    {
      var textHtml = BuildPostHtml(
        $"""
        <p><a href="https://www.reddit.com/r/HFY/comments/1exzyx5/my_example_story_100/">{firstLinkLabel}</a></p>
        """,
        """
        <p><a href="https://www.reddit.com/r/HFY/comments/1exzyx5/my_example_story_100/">First</a></p>
        """
      );

      SelfPost post = new(null, "HFY", "My Example Story", "pumbas600", null, textHtml, "sdfghj");

      var chapter = chapterParsingService.ChapterFromPost(post);

      Assert.Equal("My Example Story", chapter.Title);
      Assert.Equal("sdfghj", chapter.Id);
      Assert.Equal(BuildPostHtml("<p></p>", "<p></p>"), chapter.TextHtml);
      Assert.Equal("1exzyx5", chapter.FirstChapterId);
      Assert.Null(chapter.NextChapterId);
      Assert.Null(chapter.PreviousChapterId);
    }

    [Fact]
    public void ChapterFromPost_WithNoLinks_ReturnsChapter()
    {
      var textHtml = BuildPostHtml();

      SelfPost post = new(null, "HFY", "My Example Story", "pumbas600", null, textHtml, "sdfghj");

      var chapter = chapterParsingService.ChapterFromPost(post);

      Assert.Equal("My Example Story", chapter.Title);
      Assert.Equal("sdfghj", chapter.Id);
      Assert.Equal(BuildPostHtml(), chapter.TextHtml);
      Assert.Null(chapter.PreviousChapterId);
      Assert.Null(chapter.NextChapterId);
    }

    private static string BuildPostHtml(string prefixHtml = "", string suffixHtml = "")
    {
      return $"""
      <div class="md">
        {prefixHtml}
        <p>
          <a href="https://www.patreon.com/sdfsdfsdf"> Patreon </a> |
          <a href="https://www.reddit.com/r/MySubreddit/"> Official Subreddit </a> |
          <a href="https://www.royalroad.com/fiction/70060/my_example_story"> Royal Road </a>
        </p>

        <p><strong>Example text...</strong></p>
        {suffixHtml}
      </div>
      """;
    }
  }
}
