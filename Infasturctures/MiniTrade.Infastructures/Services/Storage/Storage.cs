using MiniTrade.Infastructures.StaticServices;


namespace MiniTrade.Infastructures.Services.Storage
{
  public  class Storage
  {
    protected delegate bool HasFile(string pathOrContainer, string fileName);
    protected async Task<string> FileRenameAsync(string pathOrContainer, string fileName, HasFile hasFileMethod, bool first = true)
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
            string fileNo = newName.Substring(index1, index2 - index1 - 1);
            if (int.TryParse(fileNo, out int _fileNo))
            {
              _fileNo++;
              newName = newName.Remove(index1 + 1, index2 - index1 - 1).Insert(index1 + 1, _fileNo.ToString());

            }
            else
            {
              newName = $"{Path.GetFileNameWithoutExtension(fileName)}{extension}";

            }
          }
        }
        //if (!File.Exists($"{pathOrContainer}\\{newName}"))
        if (hasFileMethod(pathOrContainer, newName))
        {
          return await FileRenameAsync(pathOrContainer, newName,hasFileMethod, false);
        }
        else return newName;
      });
      return newFileName;
    }
  }
}
