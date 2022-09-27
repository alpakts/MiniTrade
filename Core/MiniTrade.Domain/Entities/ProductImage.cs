using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Domain.Entities
{
  public class ProductImage:File
  {
    public ICollection<Product> Products { get; set; }
  }
}
