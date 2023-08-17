using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Script
{
    public static class ApiHelper
    {
        public static async Task<string> DownloadFile(this HttpClient client,string url,string filePath,string fileName,string extension)
        {
            using var s = await client.GetStreamAsync(new Uri(url));
            string directoryPath = FileHelper.GetApplicationPath() + filePath;
            FileHelper.DirectoryCheck(directoryPath);
            string totalPath = directoryPath + fileName + "." + extension;

            if (File.Exists(totalPath))
                return totalPath;
            
            using var fs = new FileStream(totalPath, FileMode.CreateNew);
            await s.CopyToAsync(fs);
            return totalPath;
        }
    }

    public static class FileHelper
    {
        public static string GetApplicationPath()
        {
            return Application.dataPath + "/";
        }
        public static void DirectoryCheck(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}