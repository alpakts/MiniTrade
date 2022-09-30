using MediatR;
using MiniTrade.Application.Abstractions.Services.User.Authentication;
using MiniTrade.Application.Abstractions.Token;

namespace MiniTrade.Application.Features.Commands.User
{
  public class LoginWithGoogleCommandRequest : IRequest<LoginWithGoogleCommandResponse>
  {
    public string? Id { get; set; }
    public string? IdToken { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? PhotoUrl { get; set; }
    public string? Provider { get; set; }

    public class LoginWithGoogleCommandHandler : IRequestHandler<LoginWithGoogleCommandRequest, LoginWithGoogleCommandResponse>
    {
      IExternalAuthService _AuthService;

      public LoginWithGoogleCommandHandler(IExternalAuthService authService)
      {
        _AuthService = authService;
      }

      public async Task<LoginWithGoogleCommandResponse> Handle(LoginWithGoogleCommandRequest request, CancellationToken cancellationToken)
      {
        Token token = await _AuthService.GoogleLoginAsync(request.IdToken, request.Provider, 60);
        return new()
        {
          Token = token
        };
      }
    }
  }
  public class LoginWithGoogleCommandResponse
  {
    public Token Token { get; set; }
  }
}
