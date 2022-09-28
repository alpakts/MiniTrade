using MediatR;
using MiniTrade.Application.Repositories;
using MiniTrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Features.Queries.Product
{
  public class GetProductByIdQueryRequest:IRequest<GetProductByIdQueryResponse>
  {
    public string Id { get; set; }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
      private readonly IProductReadRepository _readRepository;

      public GetProductByIdQueryHandler(IProductReadRepository readRepository)
      {
 
        _readRepository = readRepository;
      }

      public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
      {
       Domain.Entities.Product product = await _readRepository.GetByIdAsycn(request.Id);

        return new()
        {
          Product = product
        };
      }
    }
  }
  public class GetProductByIdQueryResponse
  {
    public Domain.Entities.Product Product { get; set; }
  }
}
