using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniTrade.Application.Repositories;
using MiniTrade.Application.Repositories.File.ProductImages;
using MiniTrade.Application.Services.Storage;
using MiniTrade.Application.ViewModels.Products;
using MiniTrade.Domain.Entities;
using MiniTrade.Infastructures.Services.FileService;
using System.Net;

namespace MiniTrade.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    private readonly IProductWriteRepository _productWrite;
    private readonly IProductReadRepository _productRead;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IProductImageWriteRepository _productImageWriteRepository;
    private readonly IProductImageReadRepository _productImageReadRepository;
    private readonly IStorageService _storageService;

 

    public ProductsController(IProductWriteRepository productWrite,
      IStorageService storage,
      IProductReadRepository productRead,
      IWebHostEnvironment webHostEnvironment,
      IProductImageWriteRepository productImageWriteRepository,
      IProductImageReadRepository productImageReadRepository)
      
    {
      _productWrite = productWrite;
      _productRead = productRead;
      _webHostEnvironment = webHostEnvironment;

      _productImageWriteRepository = productImageWriteRepository;
      _productImageReadRepository = productImageReadRepository;
      _storageService = storage;
    }
    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {

      return  Ok(_productRead.GetAll(false));
    }
    [HttpGet("getid")]
    public async Task<IActionResult> getById(string id)
    {
      var product = await _productRead.GetByIdAsycn(id, false);
      return Ok(product);
    }
    [HttpPost("add")]
    public async Task<IActionResult> Add(VMCreateProduct model)
    {
      await _productWrite.AddAsync(new()
      {
        Name = model.Name,
        Stock = model.Stock,
        Price = model.Price,
      });
      await _productWrite.SaveAsync();
      return StatusCode((int)HttpStatusCode.Created);
    }
    [HttpPut("put")]
    public async Task<IActionResult> Put(VMUpdateProduct model)
    {
      Product prod = await _productRead.GetByIdAsycn(model.Id);

      prod.Name = model.Name;
      prod.Stock = model.Stock;
      prod.Price = model.Price;
      await _productWrite.SaveAsync();
      return Ok();
    }
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(VMUpdateProduct model)
    {
      Product prod = await _productRead.GetByIdAsycn(model.Id);

      await _productWrite.RemoveAsync(model.Id);
      await _productWrite.SaveAsync();
      return Ok();
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Upload()
    {
      var result = await _storageService.UploadAsync("Products",Request.Form.Files);
      await _productImageWriteRepository.AddRangeAsync(result.Select(f => new ProductImage
      {
        FileName = f.fileName,
        Path = f.filePath,
        StorageType=_storageService.StorageName
      }).ToList());
      await _productImageWriteRepository.SaveAsync();
      return Ok(result);
    }
    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetImages(string id)
    {
      Product? product= await _productRead.Table.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
      return Ok(product?.Images.Select(p=>new {p.Path,p.FileName,p.Id}));
    }
    [HttpDelete("[action]/{productId}")]
    public async Task<IActionResult> DeleteImage(string productId, string imageId)
    {
      Product? product = await _productRead.Table.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId));
      var image =  product?.Images.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
      await _productImageWriteRepository.RemoveAsync(image.Id.ToString());
      await _productImageWriteRepository.SaveAsync();
      return Accepted();
    }
  }

}
