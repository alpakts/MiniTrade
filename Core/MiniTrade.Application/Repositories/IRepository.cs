using Microsoft.EntityFrameworkCore;
using MiniTrade.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
