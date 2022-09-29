using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using MiniTrade.Application.Features.Commands.Product;
using MiniTrade.Application.Repositories;


namespace MiniTrade.Application.Features.Queries.Product.ProductImages
{
  public class GetProductImagesQueryRequest : IRequest<List<GetProductImagesQueryResponse>>
  {
    public string Id { get; set; }

    public class GetPRoductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
    {
      private readonly IProductReadRepository _productReadRepository;
      private readonly IConfiguration configuration;


      public GetPRoductImagesQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
      {
        _productReadRepository = productReadRepository;
        this.configuration = configuration;
      }

      public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
      {
        MiniTrade.Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.Images)
             .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));
        if (product != null)
        {
          List<GetProductImagesQueryResponse> images = product.Images.Select(p => new GetProductImagesQueryResponse
          {
           Path = $"{configuration["BaseStorageUrl"]}/{p.Path}",
            FileName=p.FileName,
            Id=p.Id
          }).ToList();
          return images;
        }
        return new();      }
    }
   
  }
  public class GetProductImagesQueryResponse
  {
    public Guid? Id { get; set; }
    public string? Path { get; set; }
    public string? FileName { get; set; }
  }
}
