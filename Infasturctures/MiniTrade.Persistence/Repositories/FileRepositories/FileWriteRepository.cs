using MiniTrade.Application.Repositories.File;
using MiniTrade.Persistence.Contexts;

namespace MiniTrade.Persistence.Repositories.FileRepositories
{
  public class FileWriteRepository : WriteRepository<Domain.Entities.File>, IFileWriteRepository
  {
    public FileWriteRepository(MiniTradeAPIDbContext context) : base(context)
    {
    }
  }
}
