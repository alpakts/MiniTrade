using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MiniTrade.Application.Repositories;
using MiniTrade.Domain.Entities.Common;
using MiniTrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity, new()
    {
        private readonly MiniTradeAPIDbContext _context;

        public WriteRepository(MiniTradeAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> model)
        {
            await Table.AddRangeAsync(model);
            return true;
        }

        public bool Remove(T model)
        {
            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool>   RemoveAsync(string id)
        {
            T model=await Table.FirstOrDefaultAsync(data => data.Id ==Guid.Parse(id) );
            return Remove(model);
        }

        public bool RemoveRange(List<T> model)
        {
            Table.RemoveRange(model);
            return true;
        }

        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();

        public bool Update(T model)
        {
            EntityEntry<T> entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
