using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Infastructures.StaticServices
{
  public static class RenameHelper
  {
    public static string RenameFile(string name)
    {
      name.Replace("/", "");
      name.Replace("+", "");
      name.Replace("*", "");
      name.Replace("_", "");
      name.Replace("|", "");
      name.Replace("@", "");
      name.Replace(",", "");
      name.Replace("%", "");
      name.Replace("^", "");
      name.Replace("'", "");
      name.Replace("!", "");
      name.Replace("{", "");
      name.Replace("}", "");
      name.Replace("#", "");
      name.Replace(";", "");
      name.Replace(":", "");
      name.Replace("£", "");
      name.Replace("½", "");
      name.Replace("$", "");
      name.Replace(">", "");
      name.Replace("<", "");
      name.Replace("İ", "i");
      name.Replace("Ö", "o");
      name.Replace("ö", "o");
      name.Replace("ı", "i"); 
      name.Replace("Ü", "u");
      name.Replace("ü", "ü");
      name.Replace("ç", "c");
      name.Replace("ş", "s");
      name.Replace("Ş", "s");
      name.Replace("Ç", "c");
      return name;
    }
  }
}
