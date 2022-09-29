using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniTrade.Application.Abstractions.Token;
using MiniTrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Features.Commands.Users
{
  public  class LoginCommandRequest:IRequest<LoginCommandResponse>
  {
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }
    

    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
      private readonly ITokenHandler _tokenHandler;
      private readonly UserManager<AppUser> _userManager;
      private readonly SignInManager<AppUser> _signInManager;

      public LoginCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler)
      {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
      }

      public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
      {
        AppUser user=await _userManager.FindByNameAsync(request.UserNameOrEmail);
        if (user == null)
          user= await _userManager.FindByEmailAsync(request.UserNameOrEmail);
        if (user == null) throw new Exception("kullanıcı adı yada şifre hatalı");
        
        var result=await _signInManager.CheckPasswordSignInAsync(user, request.Password,false);
        if (result.Succeeded)// authentication başarılı 
        {
          var Token=_tokenHandler.CreateAccessToken(15);
          return  new LoginCommandSuccessResponse()
          {
            Token = Token
          };
        }
        return new LoginCommandErrorResponse()
        {
          Message = "Kullanıcı adı ya da şifre hatalı"
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
