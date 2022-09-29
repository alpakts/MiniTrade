using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Domain.Exceptions
{
  public class UserCreateFailedException:Exception
  {
    public UserCreateFailedException():base("Kullanıcı Eklenilirken Beklemeyen Bir Hata İle karşılaşıldı")
    {
    }

    public UserCreateFailedException(string? message, Exception? innerException) : base(message, innerException)
    {

    }

    protected UserCreateFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
