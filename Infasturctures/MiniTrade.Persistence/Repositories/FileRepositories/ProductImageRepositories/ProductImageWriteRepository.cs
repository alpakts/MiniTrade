using MiniTrade.Application.Repositories.File;
using MiniTrade.Application.Repositories.File.ProductImages;
using MiniTrade.Domain.Entities;
using MiniTrade.Persistence.Contexts;

namespace MiniTrade.Persistence.Repositories.FileRepositories.ProductImageRepositories
{
  public class ProductImageWriteRepository : WriteRepository<ProductImage>, IProductImageWriteRepository
  {
    public ProductImageWriteRepository(MiniTradeAPIDbContext context) : base(context)
    {
    }
  }
}
