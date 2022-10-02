using MediatR;
using Microsoft.Extensions.Logging;
using MiniTrade.Application.Repositories;

namespace MiniTrade.Application.Features.Commands.Product
{
  

  public  class UpdateProductCommandRequest:IRequest<UpdateProductCommandResponse>
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public float  Price { get; set; }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
      private readonly IProductReadRepository _readRepository;
      private readonly IProductWriteRepository _writeRepository;
      private readonly ILogger<UpdateProductCommandHandler> _logger;

      public UpdateProductCommandHandler(IProductReadRepository readRepository, IProductWriteRepository writeRepository)
      {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
      }

      public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
      {
        MiniTrade.Domain.Entities.Product product = await _readRepository.GetByIdAsycn(request.Id);
        product.Stock = request.Stock;
        product.Name = request.Name;
        product.Price = request.Price;
        await _writeRepository.SaveAsync();
        _logger.LogInformation("Product Updated");
        return new()
        { Product = product };
      }
    }
  }
  public class UpdateProductCommandResponse
  {
    public Domain.Entities.Product Product { get; set; }
  }
}
