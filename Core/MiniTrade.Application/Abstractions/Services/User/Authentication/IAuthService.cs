using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Abstractions.Services.User.Authentication
{
  public interface IAuthService:IInternalAuthService,IExternalAuthService
  {
  }
}
