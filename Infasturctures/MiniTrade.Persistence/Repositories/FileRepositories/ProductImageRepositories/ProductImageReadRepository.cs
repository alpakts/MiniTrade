using MiniTrade.Application.Repositories.File.ProductImages;
using MiniTrade.Domain.Entities;
using MiniTrade.Persistence.Contexts;

namespace MiniTrade.Persistence.Repositories.FileRepositories.ProductImageRepositories
{
  public class ProductImageReadRepository : ReadRepository<ProductImage>, IProductImageReadRepository
  {
    public ProductImageReadRepository(MiniTradeAPIDbContext context) : base(context)
    {
    }
  }
}
