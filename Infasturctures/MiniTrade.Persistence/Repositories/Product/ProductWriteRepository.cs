using MiniTrade.Application.Repositories;
using MiniTrade.Domain.Entities;
using MiniTrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Persistence.Repositories
{
    public class ProductWriteRepository : WriteRepository<Product>, IProductWriteRepository
    {
        public ProductWriteRepository(MiniTradeAPIDbContext context) : base(context)
        {
        }
    }
}
