using MediatR;
using MiniTrade.Application.Repositories;
using MiniTrade.Domain.Entities;

namespace MiniTrade.Application.Features.Commands.Product
{
    public  class AddProductCommandRequest:IRequest<AddProductCommandResponse>
  {
        public Domain.Entities.Product? Product { get; set; }
        public class AddProductCommandHandler : IRequestHandler<AddProductCommandRequest, AddProductCommandResponse>
    {
      readonly IProductWriteRepository _writeRepository;

      public AddProductCommandHandler(IProductWriteRepository writeRepository)
      {
        _writeRepository = writeRepository;
      }

      public async Task<AddProductCommandResponse> Handle(AddProductCommandRequest request, CancellationToken cancellationToken)
      {

        bool result=await _writeRepository.AddAsync(new Domain.Entities.Product()
        {
          Name = request.Product.Name,
          Price = request.Product.Price,
          Stock = request.Product.Stock,
          CreateDate = DateTime.Now,
          UpdateDate = DateTime.Now,


        });
        await _writeRepository.SaveAsync();
        return new()
        {
          IsSuccess = result
        };
      }
    }
  }
  public class AddProductCommandResponse
  {
    public bool IsSuccess { get; set; }
  }
}
