using Microsoft.EntityFrameworkCore;
using MiniTrade.Domain.Entities;
using MiniTrade.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Persistence.Contexts
{
    public class MiniTradeAPIDbContext : DbContext
    {
        public MiniTradeAPIDbContext(DbContextOptions options) : base(options)
        {}
        //Entity framework databaseye entity modellerini tanımlama
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
    public DbSet<Domain.Entities.File> Files { get; set; }
    public DbSet<InvoiceFile> InvoiceFiles { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //Change tracker : Entityler üzerinden yapılan değişikliklerin  ya da
            // yeni eklenen verinin yakalanmasını sağlayan propertyidir.update operasyonlarında Track takip edilen verileri yapalayıp elde etmemizi sağlar
            var datas=ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                // discard yapılanması ile geri dönüşü kapatır '_' ile kullanılır
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreateDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdateDate = DateTime.UtcNow
                };

            }
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
