using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTrade.Application.Features.Commands.Product;
using MiniTrade.Application.Features.Commands.Product.ProductImages;
using MiniTrade.Application.Features.Queries.Product;
using MiniTrade.Application.Features.Queries.Product.ProductImages;

using System.Net;

namespace MiniTrade.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(AuthenticationSchemes="admin")]
  public class ProductsController : ControllerBase
  {
    readonly IMediator _mediator;

    public ProductsController(
      IMediator mediator)
    {

      _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
    {
      var response = await _mediator.Send(getAllProductQueryRequest);
      return Ok(response);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] GetProductByIdQueryRequest getProductByIdQueryRequest)
    {
      var response = await _mediator.Send(getProductByIdQueryRequest);
      return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post(AddProductCommandRequest createProductCommandRequest)
    {
      AddProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
      if (response.IsSuccess)
      {
        return StatusCode((int)HttpStatusCode.Created);
      }
      return StatusCode((int)HttpStatusCode.BadRequest);

    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
    {
      var response = await _mediator.Send(updateProductCommandRequest);
      return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteProductCommandRequest deleteProductCommandRequest)
    { 
      var response = await _mediator.Send(deleteProductCommandRequest);
      return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Upload([FromQuery] ProductImageUploadCommandRequest productImageUploadCommandRequest )
    {
      productImageUploadCommandRequest.FormFileCollection = Request.Form.Files;
      var response = await _mediator.Send(productImageUploadCommandRequest);
      return Ok(response);
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
    {
      var response=await  _mediator.Send(getProductImagesQueryRequest);
      return Ok(response);
    }
    [HttpDelete("[action]/{Id}")]
    public async Task<IActionResult> DeleteProductImage([FromRoute] DeleteProductImageCommandRequest deleteProductImageCommandRequest, [FromQuery] string imageId)
    {
      deleteProductImageCommandRequest.ImageId = imageId;
      var response = _mediator.Send(deleteProductImageCommandRequest);
      return Ok(response);
    }

  }
  

}
