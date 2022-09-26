using MiniTrade.Application.Repositories.File.InvoiceFiles;
using MiniTrade.Domain.Entities;
using MiniTrade.Persistence.Contexts;

namespace MiniTrade.Persistence.Repositories.FileRepositories.InvoiceFileRepositores
{
  public class InvoiceFileReadRepository : ReadRepository<InvoiceFile>, IInvoiceReadRepository
  {
    public InvoiceFileReadRepository(MiniTradeAPIDbContext context) : base(context)
    {
    }

  }
}

