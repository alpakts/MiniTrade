using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MiniTrade.Application.Abstractions.Token;
using MiniTrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Features.Commands.User
{
  public class LoginWithGoogleCommandRequest:IRequest<LoginWithGoogleCommandResponse>
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
      private readonly UserManager<AppUser> _userManager;
      private readonly ITokenHandler _tokenHandler;
      private readonly IConfiguration _configuration;

      public LoginWithGoogleCommandHandler(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration)
      {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _configuration = configuration;
      }
      public async Task<LoginWithGoogleCommandResponse> Handle(LoginWithGoogleCommandRequest request, CancellationToken cancellationToken)
      {
        var setting = new GoogleJsonWebSignature.ValidationSettings()
        {
          Audience = new List<string> { _configuration["LoginProviders:Google"] },

        };
        var payload= await GoogleJsonWebSignature.ValidateAsync(request.IdToken, setting);
        var info=new UserLoginInfo(request.Provider, payload.Scope, request.Provider);
        var user=await _userManager.FindByLoginAsync(info.LoginProvider,info.ProviderKey);
        bool result = user == null;
        if (user==null)
        {
          user = await _userManager.FindByEmailAsync(payload.Email);
          if (user==null)
          {
            user = new()
            {
              Id = Guid.NewGuid().ToString(),
              UserName = payload.Name,
              Email = payload.Email,
              NameSurname = payload.Name,
              
            };
            var createResult =await _userManager.CreateAsync(user);
            result = createResult.Succeeded;
          }

        }
        if (result)
        {
          await _userManager.AddLoginAsync(user, info);
        }
        else
        {
          throw new Exception("İnvalid External Authentication");
        }
        var token = new Token();
        token = _tokenHandler.CreateAccessToken(15);
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
