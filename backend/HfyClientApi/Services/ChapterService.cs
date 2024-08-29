using HfyClientApi.Dtos;
using HfyClientApi.Exceptions;
using HfyClientApi.Models;
using HfyClientApi.Repositories;
using HfyClientApi.Utils;
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

    public async Task<Result<FullChapterDto>> GetChapterByIdAsync(string id)
    {
      var chapterResult = await _chapterRepository.GetChapterByIdAsync(id);
      return chapterResult.Map(_mapper.ToFullChapterDto);
    }

    public async Task<Result<FullChapterDto>> ProcessChapterByIdAsync(string id)
    {
      var postFullname = $"t3_{id}";
      var posts = _reddit.GetPosts([postFullname]);
      if (posts.Count == 0)
      {
        return Errors.PostNotFound(id);
      }

      var post = posts[0];
      if (post is not SelfPost selfPost)
      {
        return Errors.PostNotSelfPost(id);
      }

      var parsedChapter = _chapterParsingService.ChapterFromPost(selfPost);


      if (parsedChapter.PreviousChapterId == null && parsedChapter.IsFirstChapter)
      {
        Story story = new()
        {
          Author = selfPost.Author,
          Subreddit = selfPost.Subreddit,
        };

        var createdChapterResult = await _chapterRepository.UpsertFirstChapterAsync(story, parsedChapter);
        return createdChapterResult.Map(_mapper.ToFullChapterDto);
      }
      // TODO: Validate that the previous chapter link exists
      else if (parsedChapter.FirstChapterId != null)
      {
        throw new NotImplementedException();
      }
      else
      {
        throw new NotImplementedException();
      }
    }
  }
}
