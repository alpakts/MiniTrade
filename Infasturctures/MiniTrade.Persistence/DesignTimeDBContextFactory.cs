using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MiniTrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Persistence
{
    public class DesignTimeDBContextFactory : IDesignTimeDbContextFactory<MiniTradeAPIDbContext>
    {
        //dotnet cli ile migration yapmak için gerekli design time classı
        public MiniTradeAPIDbContext CreateDbContext(string[] args)
        {
            
            DbContextOptionsBuilder<MiniTradeAPIDbContext> builder = new();
            builder.UseSqlServer(Configuration.ConnectionString);
            return new MiniTradeAPIDbContext(builder.Options);
            
        }
    }
}
