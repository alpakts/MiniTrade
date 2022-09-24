using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MiniTrade.Application.Services.FileServicee;
using MiniTrade.Infastructures.StaticServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//todo dosya yükleme işlemi bitirilecek
namespace MiniTrade.Infastructures.Services.FileService
{
  public class FileService : IFileService
  {
    private readonly IWebHostEnvironment _webHost;

    public FileService(IWebHostEnvironment webHost)
    {
      _webHost = webHost;
    }
    public async Task<List<(string fileName, string filePath)>> UploadAsync(string path, IFormFileCollection files)
    {
      string uploadPath = Path.Combine(_webHost.WebRootPath, path);
      if (!Directory.Exists(uploadPath))
        Directory.CreateDirectory(uploadPath);
      List<(string fileName, string filePath)> datas = new();
      List<bool> results = new();
      foreach (IFormFile file in files)
      {
        string fileNewName = await FileRenameAsync(uploadPath, file.FileName);
        bool result = await SaveFileAsync(Path.Combine(uploadPath, fileNewName), file);
        results.Add(result);
        datas.Add((fileNewName, Path.Combine(uploadPath, fileNewName)));
      }
      
      if (results.TrueForAll(r => r.Equals(true)))
        return datas;
      return null;

      //todo eğerki if geçerli değilse sunucuya yüklenirken bir hatanın alındığı alanda bir exception oluşturulup fırlatılacak

    }



    public async Task<bool> SaveFileAsync(string path, IFormFile files)
    {
      try
      {
        using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, false);
        await files.CopyToAsync(fileStream);
        await fileStream.FlushAsync();
        return true;
      }
      catch (Exception)
      {

        //todo log!
        throw;
      }


    }
    private async Task<string> FileRenameAsync(string path, string fileName, bool first = true)
    {
      string newFileName = await Task.Run<string>(async () =>
      {
        string extension = Path.GetExtension(fileName);
        string newName = string.Empty;
        if (first)
        {
          string oldname = Path.GetFileNameWithoutExtension(fileName);
          newName = $"{RenameHelper.RenameFile(oldname)}{extension}";
        }
        else
        {
          newName = fileName;
          int index1 = newName.LastIndexOf("-");

          if (index1 == -1)
          {
            newName = $"{Path.GetFileNameWithoutExtension(fileName)}-2{extension}";
          }
          else
          {
            int index2 = newName.LastIndexOf(".");
            string fileNo = newName.Substring(index1, index2 - index1-1);
            if (int.TryParse(fileNo, out int _fileNo))
            {
              _fileNo++;
              newName = newName.Remove(index1+1, index2 - index1-1).Insert(index1+1, _fileNo.ToString());

            }
            else
            {
              newName = $"{Path.GetFileNameWithoutExtension(fileName)}{extension}";

            }
          }
        }

        if (!File.Exists($"{path}\\{newName}"))
        {
          return await FileRenameAsync(path, newName, false);
        }
        else return newName;
      });
      return newFileName;
    }
  }
}
