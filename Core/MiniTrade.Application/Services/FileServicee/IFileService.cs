using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTrade.Application.Services.FileServicee
{
  public interface IFileService
  {
    Task<List<(string fileName, string filePath)>> UploadAsync(string path, IFormFileCollection files);
    
    Task<bool> SaveFileAsync(string path, IFormFile files);
  }
}
