using MiniTrade.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Domain.Entities
{
  public class Product : BaseEntity
  {
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<ProductImage> Images { get; set; }
  }
}
