using MediatR;
using MiniTrade.Application.Repositories;
using MiniTrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MiniTrade.Application.Features.Queries.GetAllProduct.GetAllProductQueryRequest;

namespace MiniTrade.Application.Features.Queries.GetAllProduct
{
  public class GetAllProductQueryRequest : IRequest<GetAllProductQueryResponse>
  {
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
    public class GetAllProductQuerHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
      readonly IProductReadRepository _productReadRepository;
      public GetAllProductQuerHandler(IProductReadRepository productReadRepository)
      {
        _productReadRepository = productReadRepository;
      }
      public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
      {
        var totalCount = _productReadRepository.GetAll(false).Count();
        var products = _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size).Select(p => new
        {
          p.Id,
          p.Name,
          p.Stock,
          p.Price,
          p.CreateDate,
          p.UpdateDate
          

        }).ToList();

        return new()
        {
          Products = products,
          TotalCount = totalCount
        };
      }
    }
    public class GetAllProductQueryResponse
    {
      public int TotalCount { get; set; }
      public object Products { get; set; }
    }
  }
}
