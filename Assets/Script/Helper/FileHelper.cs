using System.Collections.Generic;
using System.IO;
using System.Linq;
using Script.Helper;
using Script.Manager;
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

        public static string GetCardsLibraryPath()
        {
            string path = GetApplicationPath() + "Card_Library/";
            DirectoryCheck(path);
            return path;
        }

        public static List<CardNameData> GetCardsInLibrary()
        {
            string libraryPath = GetCardsLibraryPath();
            List<string> cardsFiles = Directory.GetFiles(libraryPath).Where(s => s.Contains(".jpg")).ToList();
            List<CardNameData> cardsNameDatas = new List<CardNameData>();

            char[] separator = new char[2];
            separator[0] = '~';
            separator[1] = '.';
            
            for (int i = 0; i < cardsFiles.Count; i++)
            {
                string cardFileName = cardsFiles[i].ToFileName();
                string[] cardSplit = cardFileName.Split(separator);
                cardsNameDatas.Add(new CardNameData(cardSplit[0],cardSplit[1],cardFileName,cardsFiles[i]));
            }

            return cardsNameDatas;
        }
    }
}