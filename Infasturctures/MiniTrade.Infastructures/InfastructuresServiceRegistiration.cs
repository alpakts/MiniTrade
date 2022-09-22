using Microsoft.Extensions.DependencyInjection;
using MiniTrade.Application.Services.FileServicee;
using MiniTrade.Infastructures.Services.FileService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Infastructures
{
  public static  class InfastructuresServiceRegistiration
  {
    public static void AddInfastructuresServices(this IServiceCollection services)
    {
      services.AddScoped<IFileService, FileService>();
      return services;
    }
  }
}
