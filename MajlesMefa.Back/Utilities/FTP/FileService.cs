using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MajlesMefa.Back.Utilities.FTP
{
    public class FileService : IFileService
    {
        async Task IFileService.SaveFileToServerAsync(string pathe, IFormFile postedFile)
        {
            if (postedFile.Length > 0)
            {
                using (var stream = new FileStream(pathe, FileMode.Create))
                {
                    await postedFile.CopyToAsync(stream);
                }
            }
        }
        public bool FileExist(string filePathe)
        {
            return File.Exists(filePathe);
        }
        public void DeleteFileFromServer(string filePathe)
        {
            if (File.Exists(filePathe))
            {
                File.Delete(filePathe);
            }
        }

        public void DeleteFileListFromServer(List<string> fileList, string location)
        {
            var files = Directory.GetFiles(location)
                .Where(x => fileList.Contains(Path.GetFileName(x)));
            foreach (var file in files.Where(File.Exists))
            {
                File.Delete(file);
            }
        }
    }
}
