
using MiniTrade.Application.Repositories.File.InvoiceFiles;
using MiniTrade.Domain.Entities;
using MiniTrade.Persistence.Contexts;

namespace MiniTrade.Persistence.Repositories.FileRepositories.InvoiceFileRepositores
{
  public class InvoiceFileWriteRepository : WriteRepository<InvoiceFile>, IInvoiceWriteRepository
  {
    public InvoiceFileWriteRepository(MiniTradeAPIDbContext context) : base(context)
    {
    }
  }
}


