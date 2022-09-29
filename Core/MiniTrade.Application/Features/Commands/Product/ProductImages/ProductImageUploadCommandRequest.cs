using MediatR;
using Microsoft.AspNetCore.Http;
using MiniTrade.Application.Abstractions.Storage;
using MiniTrade.Application.Repositories;
using MiniTrade.Application.Repositories.File.ProductImages;
using MiniTrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Features.Commands.Product.ProductImages
{
  public  class ProductImageUploadCommandRequest:IRequest<ProductImageUploadCommandResponse>
  {
    public string? Id { get; set; }
    public IFormFileCollection? FormFileCollection { get; set; }
    public class ProductImageUploadCommandHandler : IRequestHandler<ProductImageUploadCommandRequest, ProductImageUploadCommandResponse>
    {
      IProductImageWriteRepository _productImageWriteRepository;
      IProductReadRepository _productReadRepository;
      IStorageService _storageService;

      public ProductImageUploadCommandHandler(IProductImageWriteRepository productImageWriteRepository, IProductReadRepository productReadRepository, IStorageService storageService)
      {
        _productImageWriteRepository = productImageWriteRepository;
        _productReadRepository = productReadRepository;
        _storageService = storageService;
      }

      public async Task<ProductImageUploadCommandResponse> Handle(ProductImageUploadCommandRequest request, CancellationToken cancellationToken)
      {
        List<(string fileName, string pathOrContainerName)> result = await _storageService.
          UploadAsync("photo-images", request.FormFileCollection);


        Domain.Entities.Product product = await _productReadRepository.GetByIdAsycn(request.Id);

        await _productImageWriteRepository.AddRangeAsync(result.Select(r => new ProductImage
        {
          FileName = r.fileName,
          Path = r.pathOrContainerName,
          StorageType = _storageService.StorageName,
          Products = new List<Domain.Entities.Product>() { product }
        }).ToList());

        await _productImageWriteRepository.SaveAsync();
        return new();

      }
    }
  }
  public class ProductImageUploadCommandResponse
  {

  }
}
