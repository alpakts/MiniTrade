using MediatR;
using MiniTrade.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Features.Commands.Product
{
  public class DeleteProductCommandRequest:IRequest<DeleteProductCommandResponse>
  {
    public string Id { get; set; }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
    {
      IProductWriteRepository _writeRepository;

      public DeleteProductCommandHandler(IProductWriteRepository writeRepository)
      {
        _writeRepository = writeRepository;
      }

      public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
      {
        
        bool result =await _writeRepository.RemoveAsync(request.Id);
        return new()
        {
          IsSuccess= result,
        };
      }
    }

  }
  public class DeleteProductCommandResponse
  {
    public bool IsSuccess { get; set; }
  }
}
