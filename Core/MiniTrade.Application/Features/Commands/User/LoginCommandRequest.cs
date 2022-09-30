 using MediatR;
using MiniTrade.Application.Abstractions.Services.User.Authentication;
using MiniTrade.Application.Abstractions.Token;


namespace MiniTrade.Application.Features.Commands.Users
{
  public  class LoginCommandRequest:IRequest<LoginCommandResponse>
  {
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }
    

    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
      IInternalAuthService _authService;

      public LoginCommandHandler(IInternalAuthService authService)
      {
        _authService = authService;
      }

      public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
      {
        Token token=await _authService.LoginAsync(request.UserNameOrEmail, request.Password, 60);
        return new LoginCommandSuccessResponse()
        {
          Token = token
        };
       
      }
    }

  }
  public class LoginCommandResponse
  {
    
  }
  public class LoginCommandSuccessResponse:LoginCommandResponse
  {
    public Token Token { get; set; }
  }
  public class LoginCommandErrorResponse : LoginCommandResponse
  {
    public string Message { get; set; }
  }
}
