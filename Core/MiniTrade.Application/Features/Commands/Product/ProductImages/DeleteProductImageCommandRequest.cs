using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniTrade.Application.Repositories;
using MiniTrade.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace MiniTrade.Application.Features.Commands.Product.ProductImages
{
  public class DeleteProductImageCommandRequest:IRequest<DeleteProductImageCommandResponse>
  {
    public string Id { get; set; }
    public string? ImageId  { get; set; }
    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
    {
      IProductReadRepository _productReadRepository;
      IProductWriteRepository _productWriteRepository;

      public DeleteProductImageCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
      {
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
      }

      public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
      {
        MiniTrade.Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.Images)
      .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

        ProductImage? productImageFile = product.Images.FirstOrDefault(p => p.Id == Guid.Parse(request.ImageId));
        var result=product.Images.Remove(productImageFile);
        await _productWriteRepository.SaveAsync();
        return new()
        {
          IsSuccess = result
        };
      }
    }
  }
  public class DeleteProductImageCommandResponse
  {
    public bool IsSuccess { get; set; }
  }

}
