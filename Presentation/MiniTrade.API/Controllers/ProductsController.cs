using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniTrade.Application.Abstractions.Storage;
using MiniTrade.Application.Features.Queries.GetAllProduct;
using MiniTrade.Application.Repositories;
using MiniTrade.Application.Repositories.File;
using MiniTrade.Application.Repositories.File.InvoiceFiles;
using MiniTrade.Application.Repositories.File.ProductImages;
using MiniTrade.Application.ViewModels.Products;
using MiniTrade.Domain.Entities;
namespace MiniTrade.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    readonly private IProductWriteRepository _productWriteRepository;
    readonly private IProductReadRepository _productReadRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    readonly IFileWriteRepository _fileWriteRepository;
    readonly IFileReadRepository _fileReadRepository;
    readonly IProductImageReadRepository _productImageFileReadRepository;
    readonly IProductImageWriteRepository _productImageFileWriteRepository;
    readonly IInvoiceReadRepository _invoiceFileReadRepository;
    readonly IInvoiceWriteRepository _invoiceFileWriteRepository;
    readonly IStorageService _storageService;
    readonly IConfiguration configuration;

    readonly IMediator _mediator;

    public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IProductImageReadRepository productImageFileReadRepository, IProductImageWriteRepository productImageFileWriteRepository, IInvoiceReadRepository invoiceFileReadRepository, IInvoiceWriteRepository invoiceFileWriteRepository, IStorageService storageService, IConfiguration configuration, IMediator mediator)
    {
      _productWriteRepository = productWriteRepository;
      _productReadRepository = productReadRepository;
      _webHostEnvironment = webHostEnvironment;
      _fileWriteRepository = fileWriteRepository;
      _fileReadRepository = fileReadRepository;
      _productImageFileReadRepository = productImageFileReadRepository;
      _productImageFileWriteRepository = productImageFileWriteRepository;
      _invoiceFileReadRepository = invoiceFileReadRepository;
      _invoiceFileWriteRepository = invoiceFileWriteRepository;
      _storageService = storageService;
      this.configuration = configuration;
      _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
    {
      var response = await _mediator.Send(getAllProductQueryRequest);
      return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
      return Ok(await _productReadRepository.GetByIdAsycn(id, false));
    }

    //[HttpPost]
   // public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
   // {
    //  CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
    //  return StatusCode((int)HttpStatusCode.Created);
   // }

    [HttpPut]
    public async Task<IActionResult> Put(VMUpdateProduct model)
    {
      Product product = await _productReadRepository.GetByIdAsycn(model.Id);
      product.Stock = model.Stock;
      product.Name = model.Name;
      product.Price = model.Price;
      await _productWriteRepository.SaveAsync();
      return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      await _productWriteRepository.RemoveAsync(id);
      await _productWriteRepository.SaveAsync();
      return Ok();
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Upload(string id)
    {
      List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", Request.Form.Files);


      Product product = await _productReadRepository.GetByIdAsycn(id);

      //foreach (var r in result)
      //{
      //    product.ProductImageFiles.Add(new()
      //    {
      //        FileName = r.fileName,
      //        Path = r.pathOrContainerName,
      //        Storage = _storageService.StorageName,
      //        Products = new List<Product>() { product }
      //    });
      //}

      await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImage
      {
        FileName = r.fileName,
        Path = r.pathOrContainerName,
        StorageType = _storageService.StorageName,
        Products = new List<Product>() { product }
      }).ToList());

      await _productImageFileWriteRepository.SaveAsync();

      return Ok();
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetProductImages(string id)
    {
      Product? product = await _productReadRepository.Table.Include(p => p.Images)
              .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
      return Ok(product.Images.Select(p => new
      {
        Path = $"{configuration["BaseStorageUrl"]}/{p.Path}",
        p.FileName,
        p.Id
      }));
    }
    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> DeleteProductImage(string id, string imageId)
    {
      Product? product = await _productReadRepository.Table.Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

      ProductImage? productImageFile = product.Images.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
      product.Images.Remove(productImageFile);
      await _productWriteRepository.SaveAsync();
      return Ok();
    }
  }

}
