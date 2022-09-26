using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniTrade.Application.Repositories;
using MiniTrade.Application.Repositories.File;
using MiniTrade.Application.Repositories.File.InvoiceFiles;
using MiniTrade.Persistence.Contexts;
using MiniTrade.Persistence.Repositories;
using MiniTrade.Persistence.Repositories.FileRepositories;
using MiniTrade.Persistence.Repositories.FileRepositories.InvoiceFileRepositores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Persistence
{
    public static  class PerServiceRegistiration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<MiniTradeAPIDbContext>(options => options.UseSqlServer(
               Configuration.ConnectionString));
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();
            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IInvoiceReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceWriteRepository, InvoiceFileWriteRepository>();


    }
    }
}
