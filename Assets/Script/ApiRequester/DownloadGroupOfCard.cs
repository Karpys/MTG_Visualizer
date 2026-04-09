using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Script.Helper;
using Script.Manager;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

namespace Script
{
    using System.Linq;

    public class DownloadGroupOfCard : MonoBehaviour
    {
        [SerializeField] private Color m_BorderColor = Color.black;
        [SerializeField] private Vector2Int m_BorderSize = new Vector2Int(10,10);

        private HttpClient m_Client = new HttpClient();

        public void OpenFileSelection()
        {
            string filePath = FileHelper.GetFilePath(FilterType.Text);
            
            if (filePath!=String.Empty)
            {
                _ = DownloadCards(File.ReadAllLines(filePath));
            }
            else
            {
                Debug.LogError("Select a correct file format");
            }
        }
        
        private async Task DownloadCards(string[] cards)
        {
            m_Client.DefaultRequestHeaders.Add("User-Agent", "MonApplication/1.0");
            List<JObject> cardObjects = new List<JObject>();

            for (int i = 0; i < cards.Length; i++)
            {
                string request = "https://api.scryfall.com/cards/search?q=lang:any+!" + "\"" + cards[i] + "\"";
                HttpResponseMessage response = await m_Client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var parsed = JObject.Parse(responseContent);
                    var dataArray = parsed["data"] as JArray;

                    if (dataArray == null || !dataArray.Any())
                    {
                        cards[i].Log("Wrong Data");
                        continue;
                    }
                    
                    var cardObject = JObject.FromObject(dataArray[0]);
                    cardObjects.Add(cardObject);
                    cards[i].Log("Found");
                }
            }

            foreach (JObject cardObject in cardObjects)
            {
                PreviewCardData cardData = await cardObject.ToPreviewCardData();
                //Todo : Add notif log
                cardData.cardSaveName.Log("Save");
                CardFileHelper.DownloadToLibrary(cardObject,cardData,m_BorderColor,m_BorderSize);
            }
        }
    }
}