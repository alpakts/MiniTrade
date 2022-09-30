using MediatR;
using MiniTrade.Application.Abstractions.Services.User.Authentication;
using MiniTrade.Application.Abstractions.Token;



namespace MiniTrade.Application.Features.Commands.User
{
  public class LoginWithFacebookCommandRequest : IRequest<LoginWithFacebookCommandResponse>
  {
    public string AuthToken { get; set; }
    public class LoginWithFacebookCommandHandler : IRequestHandler<LoginWithFacebookCommandRequest, LoginWithFacebookCommandResponse>
    {
      private readonly IExternalAuthService _authService;

      public LoginWithFacebookCommandHandler(IAuthService authService)
      {
        _authService = authService;
      }

      public async Task<LoginWithFacebookCommandResponse> Handle(LoginWithFacebookCommandRequest request, CancellationToken cancellationToken)
      {
        Token token = await _authService.LoginFacebookAsync(request.AuthToken,15);

        return new()
        {
          Token = token
        };

      }

    }

  }
  public class LoginWithFacebookCommandResponse
  {
    public Token? Token { get; set; }
    public bool IsSucceceed { get; set; }
  }
}
