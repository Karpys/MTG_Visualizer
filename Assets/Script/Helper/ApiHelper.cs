using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Script
{
    public static class ApiHelper
    {
        public static async Task<string> DownloadFile(this HttpClient client,string url,string filePath,string fileName,string extension,string fallBackLocation = "")
        {
            string directoryPath = CardFileHelper.GetApplicationPath() + filePath + "/";
            CardFileHelper.DirectoryCheck(directoryPath);
            string totalPath = directoryPath + fileName + "." + extension;
            
            if (File.Exists(totalPath))
            {
                await Task.Delay(1);
                return totalPath;
            }else if (fallBackLocation != "")
            {
                string fallBackPath = CardFileHelper.GetApplicationPath() + fallBackLocation + "/";
                CardFileHelper.DirectoryCheck(fallBackPath);
                fallBackPath = fallBackPath + fileName + "." + extension;

                if (File.Exists(fallBackPath))
                {
                    await Task.Delay(1);
                    return fallBackPath;
                }
            }
            
            using Stream s = await client.GetStreamAsync(new Uri(url));
            using var fs = new FileStream(totalPath, FileMode.CreateNew);
            await s.CopyToAsync(fs);
            return totalPath;
        }
        
        public static async Task<byte[]> ReadFile(this HttpClient client,string url)
        {
            byte[] fileData = await client.GetByteArrayAsync(new Uri(url));
            return fileData;
        }
    }
}