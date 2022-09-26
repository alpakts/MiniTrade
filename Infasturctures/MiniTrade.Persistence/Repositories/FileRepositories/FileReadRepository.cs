using MiniTrade.Application.Repositories.File;
using MiniTrade.Persistence.Contexts;

namespace MiniTrade.Persistence.Repositories.FileRepositories
{
  public class FileReadRepository : ReadRepository<Domain.Entities.File>, IFileReadRepository
  {
    public FileReadRepository(MiniTradeAPIDbContext context) : base(context)
    {
    }
  }
}
