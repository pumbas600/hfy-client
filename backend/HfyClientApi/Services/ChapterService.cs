using HfyClientApi.Dtos;
using HfyClientApi.Models;
using HfyClientApi.Repositories;
using Reddit;
using Reddit.Controllers;

namespace HfyClientApi.Services
{
  public class ChapterService : IChapterService
  {
    private readonly IChapterRepository _chapterRepository;
    private readonly IChapterParsingService _chapterParsingService;
    private readonly RedditClient _reddit;
    private readonly IMapper _mapper;

    public ChapterService(
      IChapterRepository chapterRepository, IChapterParsingService chapterParsingService,
      RedditClient redditClient, IMapper mapper)
    {
      _chapterRepository = chapterRepository;
      _chapterParsingService = chapterParsingService;
      _reddit = redditClient;
      _mapper = mapper;
    }

    public async Task<FullChapterDto> GetChapterByIdAsync(string id)
    {
      var chapter = await _chapterRepository.GetChapterByIdAsync(id) ?? throw new Exception($"Chapter with id {id} not found");
      return _mapper.ToFullChapterDto(chapter);
    }

    public async Task<FullChapterDto> ProcessChapterByIdAsync(string id)
    {
      var postFullname = $"t3_{id}";
      var posts = _reddit.GetPosts([postFullname]);
      if (posts.Count == 0)
      {
        throw new Exception($"Post with id {id} not found");
      }

      // Maybe need to call .About()?
      var post = posts[0];
      if (post is not SelfPost selfPost)
      {
        throw new Exception($"Post with id {id} is not a self post");
      }

      var chapter = _chapterParsingService.ChapterFromPost(selfPost);
      if (chapter.PreviousChapterId == null)
      {
        Story story = new()
        {
          Author = selfPost.Author,
          Subreddit = selfPost.Subreddit,
        };

        var createdChapter = await _chapterRepository.CreateFirstChapter(story, chapter);
        return _mapper.ToFullChapterDto(createdChapter);
      }
      else
      {
        throw new NotImplementedException();
      }
    }
  }
}
