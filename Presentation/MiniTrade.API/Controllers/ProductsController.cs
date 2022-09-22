using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniTrade.Application.Repositories;
using MiniTrade.Application.Services.FileServicee;
using MiniTrade.Application.ViewModels.Products;
using MiniTrade.Domain.Entities;
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
    private readonly IFileService _fileService;

    public ProductsController(IProductWriteRepository productWrite, IProductReadRepository productRead,IWebHostEnvironment webHostEnvironment,IFileService fileService)
    {
      _productWrite = productWrite;
      _productRead = productRead;
      _webHostEnvironment = webHostEnvironment;
      _fileService = fileService;
    }
    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {

      return Ok(_productRead.GetAll(false));
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
      await _fileService.UploadAsync("resource/product-images", Request.Form.Files);
      return Ok();
    }


  }
}