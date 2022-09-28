

using Microsoft.Extensions.DependencyInjection;
using MiniTrade.Application.Abstractions.Storage;
using MiniTrade.Infastructures.Enums;
using MiniTrade.Infastructures.Services.Storage;
using MiniTrade.Infastructures.Services.Storage.Azure;
using MiniTrade.Infastructures.Services.Storage.Local;

namespace MiniTrade.Infastructures
{
  public static class InfastructuresServiceRegistiration
  {
    public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
    {
      serviceCollection.AddScoped<IStorageService, StorageService>();
    }
    public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
    {
      serviceCollection.AddScoped<IStorage, T>();
    }
    public static void AddStorage(this IServiceCollection serviceCollection, StorageTypes storageType)
    {
      switch (storageType)
      {
        case StorageTypes.Local:
          serviceCollection.AddScoped<IStorage, LocalStorage>();
          break;
        case StorageTypes.Azure:
          serviceCollection.AddScoped<IStorage, AzureStorage>();
          break;
        case StorageTypes.AWS:

          break;
        default:
          serviceCollection.AddScoped<IStorage, LocalStorage>();
          break;
      }
    }
  }
}
