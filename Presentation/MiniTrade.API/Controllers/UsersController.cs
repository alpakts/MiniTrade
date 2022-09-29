using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniTrade.Application.Features.Commands.Users;

namespace MiniTrade.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
      _mediator = mediator;
    }
    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
    {
      var response=await _mediator.Send(createUserCommandRequest);
      return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginCommandRequest request)
    {
      var response=await _mediator.Send(request);
      return Ok(response);
    }
  }
}
