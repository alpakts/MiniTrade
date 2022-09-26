using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Services
{
  public interface IStorage
  {
    public Task DeleteAsync(string path, string fileName);
    public List<string> GetFiles(string path);
    public bool HasFile(string path, string fileName);

    public Task<List<(string fileName, string filePath)>> UploadAsync(string path, IFormFileCollection files);

  }

}

