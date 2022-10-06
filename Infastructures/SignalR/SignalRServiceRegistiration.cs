using Microsoft.Extensions.DependencyInjection;
using MiniTrade.Application.Abstractions.Hubs;
using SignalR.HubServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR
{
  public static class SignalRServiceRegistiration
  {
    public static void AddSignalRServices(this IServiceCollection services)
    {
      services.AddTransient<IProductHubService, ProductHubService>();
      services.AddSignalR();
    }
  }
}
