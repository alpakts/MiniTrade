using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Abstractions.Token
{
  public  class Token
  {
    public string? AccessToken { get; set; }
    public DateTime Expiraton { get; set; }
  }
}
