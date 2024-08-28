using HfyClientApi.Data;

namespace HfyClientApi.Repositories
{
  public class ChapterRepository
  {
    private readonly AppDbContext _context;

    public ChapterRepository(AppDbContext context)
    {
      _context = context;
    }


  }
}
