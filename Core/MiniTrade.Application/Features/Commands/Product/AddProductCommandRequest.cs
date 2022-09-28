using MediatR;
using MiniTrade.Application.Repositories;
using MiniTrade.Application.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MiniTrade.Application.Features.Commands.Product.AddProductCommandRequest;

namespace MiniTrade.Application.Features.Commands.Product
{
  public  class AddProductCommandRequest:IRequest<AddProductCommandResponse>
  {
    public VMCreateProduct  CreateProduct { get; set; }
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
          Name = request.CreateProduct.Name,
          Price = request.CreateProduct.Price,
          Stock = request.CreateProduct.Stock,
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
