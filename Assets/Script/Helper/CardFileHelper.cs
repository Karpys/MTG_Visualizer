using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Script.Helper;
using Script.Manager;
using UnityEngine;

namespace Script
{
    public static class CardFileHelper
    {
        private static HttpClient m_Client = new HttpClient();
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

        public static string GetCardsLibraryCardDataPath()
        {
            string path = GetApplicationPath() + "Card_Library/Cards/";
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


        public static List<LibraryCardData> GetCardsInLibrary()
        {
            List<LibraryCardData> cardsNameDatas = new List<LibraryCardData>();
            foreach (var value in m_CardNameLibrary.Values)
            {
                cardsNameDatas.Add(value);
            }
            return cardsNameDatas;
        }
        
        public static void UpdateLibrary()
        {
            m_CardNameLibrary.Clear();
            
            string libraryCardDataPath = GetCardsLibraryCardDataPath();
            string libraryImagePath = GetCardsLibraryPath();
            List<string> cardsFiles = Directory.GetFiles(libraryCardDataPath).Where(s => s.Contains(".card")).ToList();
            
            for (int i = 0; i < cardsFiles.Count; i++)
            {
                string cardFileName = cardsFiles[i].ToFileName();
                string[] cardData = File.ReadLines(cardsFiles[i]).ToArray();

                string imageCardPath = libraryImagePath + cardData[1] + ".jpg";
                bool isDualCard = cardData[2] != "False";
                LibraryCardData libraryCardData = new LibraryCardData(cardData[0],cardData[1],cardFileName,cardsFiles[i],imageCardPath,isDualCard);
                m_CardNameLibrary.Add(cardData[1],libraryCardData);
            }
        }

        public static Dictionary<string, LibraryCardData> m_CardNameLibrary = new Dictionary<string, LibraryCardData>();

        public static LibraryCardData CardIdToCardNameData(this string cardId)
        {
            if (m_CardNameLibrary.TryGetValue(cardId, out LibraryCardData cardNameData))
                return cardNameData;
            Debug.LogError("Not found card : " + cardId);
            return new LibraryCardData();
        }

        public static async Task<ApiCardData> ToPreviewCardData(this JObject cardObject)
        {
            string cardImageUrisFront = String.Empty;
            string cardImageUrisBack = String.Empty;
            bool isDualCard = false;

            if (cardObject["image_uris"] == null)
            {
                if (cardObject["card_faces"]?[0]?["image_uris"] != null)
                {
                    cardImageUrisFront = (string)cardObject["card_faces"][0]["image_uris"]["normal"];

                    if (cardObject["card_faces"]?[1]?["image_uris"] != null)
                    {
                        isDualCard = true;
                        cardImageUrisBack = (string)cardObject["card_faces"]?[1]?["image_uris"]["normal"];
                    }
                }
                else
                {
                    Debug.LogError("Unknown cards");
                    return new ApiCardData();
                }
            }
            else
            {
                cardImageUrisFront = (string)cardObject["image_uris"]["normal"];
            }
            
            string cardName = (string) cardObject["name"];
            
            if (cardName != null)
            {
                if (!isDualCard)
                {
                    byte[] cardImageBytes = await m_Client.ReadFile(cardImageUrisFront);
                    Sprite cardSprite = cardImageBytes.ToCardSprite();
                    ApiCardData data = JObjectToPreviewCardData(cardObject,cardSprite);
                    return data;
                }
                else
                {
                    byte[] cardImageBytesFront = await m_Client.ReadFile(cardImageUrisFront);
                    Sprite cardSpriteFront = cardImageBytesFront.ToCardSprite();
                    byte[] cardImageBytesBack = await m_Client.ReadFile(cardImageUrisBack);
                    Sprite cardSpriteBack = cardImageBytesBack.ToCardSprite();
                    ApiCardData data = JObjectToPreviewDualCardData(cardObject,cardSpriteFront,cardSpriteBack);
                    return data;
                }
            }
            else
            {
                return new ApiCardData();
            }
        }

        public static void DownloadToLibrary(JObject cardObject, ApiCardData cardData,Color borderColor,Vector2Int borderSize)
        {
            byte[] pixels = cardData.ToPixels(borderColor, borderSize);
            string filePath = GetCardsLibraryPath();
            filePath += cardData.m_CardId+".jpg";
            File.WriteAllBytes(filePath, pixels);
            DownloadCardFile(cardData);
            //Todo:Save a file ".card" contains : Name + Id + Color + Cost without X + Type//
        }

        private static byte[] ToPixels(this ApiCardData cardData, Color borderColor, Vector2Int borderSize)
        {
            if (!cardData.m_IsDualCard)
            {
                return SpriteToPixels(cardData.m_FrontCardSprite,borderColor,borderSize);
            }
            else
            {
                int cardWidth  = cardData.m_FrontCardSprite.texture.width;
                int cardHeight = cardData.m_FrontCardSprite.texture.height;
                Texture2D combinedTexture = new Texture2D(cardWidth, cardHeight * 2);
                combinedTexture.SetPixels(0, 0, cardWidth, cardHeight, cardData.m_FrontCardSprite.texture.GetPixels());
                combinedTexture.SetPixels(0, cardHeight, cardWidth, cardHeight, cardData.m_BackCardSprite.texture.GetPixels());
                combinedTexture.SetBorderDualColor(borderColor, borderSize);
                combinedTexture.Apply();
                return combinedTexture.EncodeToJPG();
            }
        }

        private static byte[] SpriteToPixels(this Sprite cardData, Color borderColor, Vector2Int borderSize)
        {
            Texture2D texture = new Texture2D(488,680);
            texture.SetPixels(cardData.texture.GetPixels());
            texture.SetBorderColor(borderColor,borderSize);
            byte[] pixels = texture.EncodeToJPG();
            return pixels;
        }

        private static void DownloadCardFile(ApiCardData cardObject)
        {
            string[] cardData = new string[3];
            cardData[0] = cardObject.m_CardName;
            cardData[1] = cardObject.m_CardId;
            cardData[2] = cardObject.m_IsDualCard.ToString();
            string filePath = GetCardsLibraryCardDataPath();
            filePath += cardObject.m_CardId+".card";
            File.WriteAllLines(filePath, cardData);
        }

        public static ApiCardData JObjectToPreviewCardData(this JObject cardObject,Sprite sprite)
        {
            ApiCardData apiCardData = new ApiCardData();
            apiCardData.m_FrontCardSprite = sprite;
            string cardSaveName = ((string) cardObject["name"]).Replace("/", "");
            apiCardData.m_CardName = cardSaveName;
            cardSaveName += "~";
            
            string cardId = (string)cardObject["id"];
            apiCardData.m_CardId = cardId;
            cardSaveName += cardId;
            apiCardData.m_CardSaveName = cardSaveName;
            return apiCardData;
        }
        
        public static ApiCardData JObjectToPreviewDualCardData(this JObject cardObject,Sprite spriteFront,Sprite spriteBack)
        {
            ApiCardData apiCardData = new ApiCardData();
            apiCardData.m_IsDualCard = true;
            apiCardData.m_FrontCardSprite = spriteFront;
            apiCardData.m_BackCardSprite = spriteBack;
            string cardSaveName = ((string) cardObject["name"]).Replace("/", "");
            apiCardData.m_CardName = cardSaveName;
            cardSaveName += "~";
            
            string cardId = (string)cardObject["id"];
            apiCardData.m_CardId = cardId;
            cardSaveName += cardId;
            apiCardData.m_CardSaveName = cardSaveName;
            return apiCardData;
        }
    }
}