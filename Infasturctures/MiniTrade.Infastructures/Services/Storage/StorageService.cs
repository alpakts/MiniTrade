using Microsoft.AspNetCore.Http;
using MiniTrade.Application.Services;
using MiniTrade.Application.Services.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Infastructures.Services.Storage
{

    public class StorageService : IStorageService
  {
    readonly IStorageService _storage;

    public StorageService(IStorageService storage)
    {
      _storage = storage;
    }

    public string StorageName { get => _storage.GetType().Name; }

    public async Task DeleteAsync(string pathOrContainerName, string fileName)
        => await _storage.DeleteAsync(pathOrContainerName, fileName);

    public List<string> GetFiles(string pathOrContainerName)
        => _storage.GetFiles(pathOrContainerName);

    public bool HasFile(string pathOrContainerName, string fileName)
        => _storage.HasFile(pathOrContainerName, fileName);

    public async Task<List<(string fileName, string filePath)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
    => await _storage.UploadAsync(pathOrContainerName, files);
  }
}


