using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Script
{
    public static class ApiHelper
    {
        public static async Task<string> DownloadFile(this HttpClient client,string url,string filePath,string fileName,string extension)
        {
            string directoryPath = FileHelper.GetApplicationPath() + filePath;
            FileHelper.DirectoryCheck(directoryPath);
            string totalPath = directoryPath + fileName + "." + extension;

            if (File.Exists(totalPath))
            {
                await Task.Delay(10);
                return totalPath;
            }
            
            using Stream s = await client.GetStreamAsync(new Uri(url));
            using var fs = new FileStream(totalPath, FileMode.CreateNew);
            await s.CopyToAsync(fs);
            return totalPath;
        }
    }
}