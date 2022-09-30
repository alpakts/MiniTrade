using MiniTrade.Application.Abstractions.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Abstractions.Services.User.Authentication
{
  public interface IExternalAuthService
  {
    Task<Token.Token> LoginFacebookAsync(string authToken,int accesTokenLifetime);
    Task<Token.Token> GoogleLoginAsync(string idToken,string provider,int accesTokenLifetime);

  }
}
