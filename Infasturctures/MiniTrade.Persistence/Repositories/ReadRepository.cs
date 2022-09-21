using Microsoft.EntityFrameworkCore;
using MiniTrade.Application.Repositories;
using MiniTrade.Domain.Entities.Common;
using MiniTrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly MiniTradeAPIDbContext _context;

        public ReadRepository(MiniTradeAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
            {
                query=query.AsNoTracking();
                
            }
            return query;
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> filter, bool tracking = true)
        {
            var query = Table.Where(filter);
            if (!tracking)
            {
                query=query.AsNoTracking();
            }
            return query;

        }

        public async Task<T> GetByIdAsycn(string id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
            {
                query = query.AsNoTracking();

            }
            return await query.FirstOrDefaultAsync(i => i.Id == Guid.Parse(id));
        }
        //=> await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
       // => await Table.FindAsync(Guid.Parse(id));

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
            {
                query = query.AsNoTracking();

            }
            return await query.FirstOrDefaultAsync(filter);
        }
        


    }
}
