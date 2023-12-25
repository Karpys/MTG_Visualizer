using System.Collections.Generic;
using System.IO;
using System.Linq;
using Script.Helper;
using Script.Manager;
using UnityEngine;

namespace Script
{
    public static class CardFileHelper
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
        
        public static string GetDeckPath()
        {
            string path = GetApplicationPath() + "Deck/";
            DirectoryCheck(path);
            return path;
        }
        
        public static string GetDeckBackCardPath()
        {
            string path = GetDeckPath() + "DeckBackCard/";
            DirectoryCheck(path);
            return path;
        }


        public static List<CardNameData> GetCardsInLibrary()
        {
            List<CardNameData> cardsNameDatas = new List<CardNameData>();
            foreach (var value in m_CardNameLibrary.Values)
            {
                cardsNameDatas.Add(value);
            }
            return cardsNameDatas;
        }
        
        public static void UpdateLibrary()
        {
            m_CardNameLibrary.Clear();
            
            string libraryPath = GetCardsLibraryPath();
            List<string> cardsFiles = Directory.GetFiles(libraryPath).Where(s => s.Contains(".jpg")).ToList();

            char[] separator = new char[2];
            separator[0] = '~';
            separator[1] = '.';
            
            for (int i = 0; i < cardsFiles.Count; i++)
            {
                string cardFileName = cardsFiles[i].ToFileName();
                string[] cardSplit = cardFileName.Split(separator);
                CardNameData cardNameData = new CardNameData(cardSplit[0],cardSplit[1],cardFileName,cardsFiles[i]);
                m_CardNameLibrary.Add(cardSplit[1],cardNameData);
            }
        }

        public static Dictionary<string, CardNameData> m_CardNameLibrary = new Dictionary<string, CardNameData>();

        public static CardNameData CardIdToCardNameData(this string cardId)
        {
            if (m_CardNameLibrary.TryGetValue(cardId, out CardNameData cardNameData))
                return cardNameData;
            Debug.LogError("Not found card : " +cardId);
            return new CardNameData();
        }
    }
}