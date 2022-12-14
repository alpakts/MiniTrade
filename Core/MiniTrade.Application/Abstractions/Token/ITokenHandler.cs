using MiniTrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Abstractions.Token
{
  public interface ITokenHandler
  {
    Token CreateAccessToken(int second, AppUser user);
    string CreateRefreshToken();
  }
}
