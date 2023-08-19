using System.IO;
using UnityEngine;

namespace Script
{
    public static class FileHelper
    {
        public static string GetApplicationPath()
        {
            return  Application.dataPath + "/../";
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