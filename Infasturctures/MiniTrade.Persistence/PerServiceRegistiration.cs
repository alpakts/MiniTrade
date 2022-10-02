using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniTrade.Application.Abstractions.Services.User;
using MiniTrade.Application.Abstractions.Services.User.Authentication;
using MiniTrade.Application.Repositories;
using MiniTrade.Application.Repositories.File;
using MiniTrade.Application.Repositories.File.InvoiceFiles;
using MiniTrade.Application.Repositories.File.ProductImages;
using MiniTrade.Domain.Entities.Identity;
using MiniTrade.Persistence.Contexts;
using MiniTrade.Persistence.Repositories;
using MiniTrade.Persistence.Repositories.FileRepositories;
using MiniTrade.Persistence.Repositories.FileRepositories.InvoiceFileRepositores;
using MiniTrade.Persistence.Repositories.FileRepositories.ProductImageRepositories;
using MiniTrade.Persistence.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Persistence
{
  public static class PerServiceRegistiration
  {
    public static void AddPersistenceServices(this IServiceCollection services)
    {
      services.AddDbContext<MiniTradeAPIDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));
      services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<MiniTradeAPIDbContext>();
      services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
      services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
      services.AddScoped<IOrderReadRepository, OrderReadRepository>();
      services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
      services.AddScoped<IProductReadRepository, ProductReadRepository>();
      services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
      services.AddScoped<IFileReadRepository, FileReadRepository>();
      services.AddScoped<IFileWriteRepository, FileWriteRepository>();
      services.AddScoped<IProductImageReadRepository, ProductImageReadRepository>();
      services.AddScoped<IProductImageWriteRepository, ProductImageWriteRepository>();
      services.AddScoped<IInvoiceReadRepository,InvoiceFileReadRepository>();
      services.AddScoped<IInvoiceWriteRepository, InvoiceFileWriteRepository>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<IExternalAuthService, AuthService>();
      services.AddScoped<IInternalAuthService, AuthService>();
    }
  }
}
