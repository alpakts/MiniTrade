

using Microsoft.Extensions.DependencyInjection;
using MiniTrade.Application.Services;
using MiniTrade.Infastructures.Enums;
using MiniTrade.Infastructures.Services.Storage;
using MiniTrade.Infastructures.Services.Storage.Azure;
using MiniTrade.Infastructures.Services.Storage.Local;

namespace MiniTrade.Infastructures
{
  public static class InfastructuresServiceRegistiration
  {
    public static void AddInfastructuresServices(this IServiceCollection services)
    {

    }
    public static void AddStorage<T>(this IServiceCollection services) where T : Storage, IStorage
    {
      services.AddScoped<IStorage, T>();
    }
    public static void AddStorage(this IServiceCollection serviceDescriptors, StorageTypes storageTypes)
    {
      switch (storageTypes)
      {
        case StorageTypes.Local:
          serviceDescriptors.AddScoped<IStorage, LocalStorage>();
          break;
        case StorageTypes.Azure:
          serviceDescriptors.AddScoped<IStorage, AzureStorage>();
          break;
        case StorageTypes.AWS:
          break;
        default:
          serviceDescriptors.AddScoped<IStorage, LocalStorage>();
          break;
      }
    }
  }
}
