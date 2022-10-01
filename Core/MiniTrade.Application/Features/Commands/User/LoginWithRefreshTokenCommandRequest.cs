using MediatR;
using MiniTrade.Application.Abstractions.Services.User.Authentication;
using MiniTrade.Application.Abstractions.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Features.Commands.User
{
  public  class LoginWithRefreshTokenCommandRequest:IRequest<LoginWithRefreshTokenCommandResponse>
  {
    public string RefreshToken { get; set; }
    public class LoginWithRefreshTokenCommandHandler : IRequestHandler<LoginWithRefreshTokenCommandRequest, LoginWithRefreshTokenCommandResponse>
    {
      private readonly IInternalAuthService _authService;

      public LoginWithRefreshTokenCommandHandler(IInternalAuthService authService)
      {
        _authService = authService;
      }

      public async Task<LoginWithRefreshTokenCommandResponse> Handle(LoginWithRefreshTokenCommandRequest request, CancellationToken cancellationToken)
      {
        Token token =await _authService.LoginWithRefreshTokenAsync(request.RefreshToken);
        return new()
        {
          Token = token
        };
      }
    }
  }
  public class LoginWithRefreshTokenCommandResponse
  {
    public Token Token { get; set; }
  }

}
