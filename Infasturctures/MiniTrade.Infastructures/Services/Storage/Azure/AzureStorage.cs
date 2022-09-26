using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MiniTrade.Application.Services.Storage.Azure;

namespace MiniTrade.Infastructures.Services.Storage.Azure
{
  public class AzureStorage : Storage, IAzureStorage
  {
    private readonly BlobServiceClient _blobServiceClient;
    private  BlobContainerClient _blobContainerClient;

    public AzureStorage(IConfiguration configuration)
    {
      _blobServiceClient = new BlobServiceClient(new(configuration["Storage:Azure"]));
     

    }


    public async Task DeleteAsync(string containerName, string fileName)
    {
      _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
      await _blobContainerClient.GetBlobClient(fileName).DeleteIfExistsAsync();
    }

    public  List<string> GetFiles(string containerName)
    {
      _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
      return _blobContainerClient.GetBlobs().Select(d => d.Name.ToString()).ToList();
    }

    public  bool HasFile(string containerName, string fileName)
    {
      _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

      return   _blobContainerClient.GetBlobClient(containerName).Exists();
    }

    public async Task<List<(string fileName, string filePath)>> UploadAsync(string containerName, IFormFileCollection files)
    {
      _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
      await _blobContainerClient.CreateIfNotExistsAsync();
      await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
      List<(string fileName, string filePath)> datas = new();
      foreach (IFormFile file in files)
      {
        string fileNewName = await FileRenameAsync(containerName, file.Name, HasFile);
        await _blobContainerClient.GetBlobClient(fileNewName).UploadAsync(file.OpenReadStream());
        datas.Add((fileNewName, containerName));
        
        
      }
      return datas;
    }
  }
}
