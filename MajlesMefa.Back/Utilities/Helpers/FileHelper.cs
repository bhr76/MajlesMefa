using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace MajlesMefa.Back.Utilities.Helpers
{
    public static class FileHelper
    {
        public static string CalculateFileHash(byte[] fileBytes)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(fileBytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static string CalculateFileHash(IFormFile formFile)
        {
            if (formFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    formFile.CopyTo(ms);
                    return CalculateFileHash(ms.ToArray());
                }
            }

            return "";
        }
    }
}