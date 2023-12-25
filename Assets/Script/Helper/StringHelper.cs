using System;
using System.Collections.Generic;
using System.IO;
using Script.UI;
using UnityEngine;

namespace Script.Helper
{
    public static class StringHelper
    {
        public static string ToFileName(this string path)
        {
            return Path.GetFileName(path);
        }

        public static DeckData ToDeckData(this string[] deckLines)
        {
            string deckName = deckLines[0];
            string deckBackPath = deckLines[1];
            Sprite backCardImage = File.ReadAllBytes(FileHelper.GetDeckBackCardPath() + deckLines[1]).ToCardSprite();
            DeckType deckType = (DeckType)int.Parse(deckLines[2]);

            List<CardCount> cardCounts = new List<CardCount>();
            for (int i = 4; i < deckLines.Length; i++)
            {
                string[] splits = deckLines[i].Split('|');
                cardCounts.Add(new CardCount(int.Parse(splits[0]), splits[1]));
            }

            return new DeckData(deckName, deckType,deckBackPath, backCardImage,cardCounts);
        }
        
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}