using MiniTrade.Domain.Entities;
using MiniTrade.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.ViewModels.Products
{
    public class VMCreateProduct:BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
