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

        public static async Task<PreviewCardData> ToPreviewCardData(this JObject cardObject)
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
                    return new PreviewCardData();
                }
            }
            else
            {
                Debug.Log((string)cardObject["name"]);
                cardImageUrisFront = (string)cardObject["image_uris"]["normal"];
            }
            
            string cardName = (string) cardObject["name"];
            
            if (cardName != null)
            {
                if (!isDualCard)
                {
                    byte[] cardImageBytes = await m_Client.ReadFile(cardImageUrisFront);
                    Sprite cardSprite = cardImageBytes.ToCardSprite();
                    PreviewCardData data = JObjectToPreviewCardData(cardObject,cardSprite);
                    return data;
                }
                else
                {
                    byte[] cardImageBytesFront = await m_Client.ReadFile(cardImageUrisFront);
                    Sprite cardSpriteFront = cardImageBytesFront.ToCardSprite();
                    byte[] cardImageBytesBack = await m_Client.ReadFile(cardImageUrisBack);
                    Sprite cardSpriteBack = cardImageBytesBack.ToCardSprite();
                    PreviewCardData data = JObjectToPreviewDualCardData(cardObject,cardSpriteFront,cardSpriteBack);
                    return data;
                }
            }
            else
            {
                return new PreviewCardData();
            }
        }

        public static void DownloadToLibrary(JObject cardObject, PreviewCardData cardData,Color borderColor,Vector2Int borderSize)
        {
            byte[] pixels = cardData.ToPixels(borderColor, borderSize);
            string filePath = GetCardsLibraryPath();
            filePath += cardData.m_CardSaveName+".jpg";
            File.WriteAllBytes(filePath, pixels);
            DownloadCardFile(cardObject,cardData.m_CardSaveName);
            //Todo:Save a file ".card" contains : Name + Id + Color + Cost without X + Type//
        }

        private static byte[] ToPixels(this PreviewCardData cardData, Color borderColor, Vector2Int borderSize)
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

        private static void DownloadCardFile(JObject cardObject,string fileName)
        {
        }

        public static PreviewCardData JObjectToPreviewCardData(this JObject cardObject,Sprite sprite)
        {
            PreviewCardData previewCardData = new PreviewCardData();
            previewCardData.m_FrontCardSprite = sprite;
            string cardSaveName = ((string) cardObject["name"]).Replace("/", "");
            previewCardData.m_CardName = cardSaveName;
            cardSaveName += "~";
            
            string cardId = (string)cardObject["id"];
            previewCardData.m_CardId = cardId;
            cardSaveName += cardId;
            previewCardData.m_CardSaveName = cardSaveName;
            return previewCardData;
        }
        
        public static PreviewCardData JObjectToPreviewDualCardData(this JObject cardObject,Sprite spriteFront,Sprite spriteBack)
        {
            PreviewCardData previewCardData = new PreviewCardData();
            previewCardData.m_IsDualCard = true;
            previewCardData.m_FrontCardSprite = spriteFront;
            previewCardData.m_BackCardSprite = spriteBack;
            string cardSaveName = ((string) cardObject["name"]).Replace("/", "");
            previewCardData.m_CardName = cardSaveName;
            cardSaveName += "~";
            
            string cardId = (string)cardObject["id"];
            previewCardData.m_CardId = cardId;
            cardSaveName += cardId;
            previewCardData.m_CardSaveName = cardSaveName;
            return previewCardData;
        }
    }
}