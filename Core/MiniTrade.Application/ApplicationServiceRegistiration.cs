using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application
{
  public static class ApplicationServiceRegistiration
  {
    public static void AddApplicationServices(this IServiceCollection collection)
    {
      collection.AddMediatR(typeof(ApplicationServiceRegistiration));
      collection.AddHttpClient();
    }
  }
}
