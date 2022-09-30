using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MiniTrade.Application
{
  public static class ApplicationServiceRegistiration
  {
    public static void AddApplicationServices(this IServiceCollection collection)
    {
      collection.AddMediatR(Assembly.GetExecutingAssembly());
      collection.AddHttpClient();
      collection.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
  }
}
