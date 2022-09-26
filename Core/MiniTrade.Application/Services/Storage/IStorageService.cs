using MiniTrade.Application.Abstractions.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Services.Storage
{
  public interface IStorageService : Abstractions.Storage.IStorage
  {
    public string StorageName { get; }


  }
}
