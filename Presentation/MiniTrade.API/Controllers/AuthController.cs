using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniTrade.Application.Features.Commands.User;
using MiniTrade.Application.Features.Commands.Users;

namespace MiniTrade.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    readonly IMediator _mediator;

    public AuthController(
      IMediator mediator)
    {

      _mediator = mediator;
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginCommandRequest request)
    {
      var response = await _mediator.Send(request);
      return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> LoginWithGoogle(LoginWithGoogleCommandRequest request)
    {
      var response = await _mediator.Send(request);
      return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> LoginWithFacebook(LoginWithFacebookCommandRequest request)
    {
      var response = await _mediator.Send(request);
      return Ok(response);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> LoginWithRefreshToken([FromQuery] LoginWithRefreshTokenCommandRequest request)
    {
      var response =await  _mediator.Send(request);
      return Ok(response);
    }
  }
}
