using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MiniTrade.Application.Abstractions.Storage.Local;
using MiniTrade.Application.Services.LocalStorage;
using ILocalStorage = MiniTrade.Application.Services.LocalStorage.ILocalStorage;

namespace MiniTrade.Infastructures.Services.Storage.Local
{
  public class LocalStorage :Infastructures.Services.Storage.Storage, ILocalStorage
  {
    private readonly IWebHostEnvironment _webHost;

    public LocalStorage(IWebHostEnvironment webHost)
    {
      _webHost = webHost;
    }
    public async Task DeleteAsync(string path, string fileName)
     => File.Delete($"{path}\\{fileName}");

    public List<string> GetFiles(string path)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(path);
      return directoryInfo.GetFiles().Select(d => d.FullName).ToList();
    }

    public bool HasFile(string path, string fileName)
     => File.Exists($"{path}\\{fileName}");
    public async Task<List<(string fileName, string filePath)>> UploadAsync(string path, IFormFileCollection files)
    {
      {
        string uploadPath = Path.Combine(_webHost.WebRootPath, path);
        if (!Directory.Exists(uploadPath))
          Directory.CreateDirectory(uploadPath);

        List<(string fileName, string path)> datas = new();
        List<bool> results = new();
        foreach (IFormFile file in files)
        {
          string fileNewName = await FileRenameAsync(path, file.Name, HasFile);

          bool result = await SaveFileAsync($"{path}\\{fileNewName}", file);
          datas.Add((fileNewName, $"{path}\\{fileNewName}"));
          results.Add(result);
        }
        //todo AWS dosya yükleme yap

        if (results.TrueForAll(r => r.Equals(true)))
          return datas;

        return null;
        //todo Eğer ki yukarıdaki if geçerli değilse burada dosyaların sunucuda yüklenirken hata alındığına dair uyarıcı bir exception oluşturulup fırlatılması gerekyior!



      }
    }
    private async Task<bool> SaveFileAsync(string path, IFormFile files)
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
  }
}
