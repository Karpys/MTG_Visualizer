using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Script.Helper;
using UnityEngine;
using File = System.IO.File;

namespace Script
{
    using System.Linq;
    using KarpysDev.KarpysUtils;

    public class DownloadGroupOfCard : MonoBehaviour
    {
        [SerializeField] private NotifLogHolder m_NotifLogHolder = null;
        [SerializeField] private Color m_BorderColor = Color.black;
        [SerializeField] private Vector2Int m_BorderSize = new Vector2Int(10,10);

        private HttpClient m_Client = new HttpClient();

        private void Awake()
        {
            m_Client.DefaultRequestHeaders.Add("User-Agent", "MonApplication/1.0");
            m_Client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

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
                if (cards[i].Length == 0)
                    continue;
                
                string request = "https://api.scryfall.com/cards/search?q=lang:any+!" + "\"" + cards[i] + "\"";
                HttpResponseMessage response = await m_Client.GetAsync(request);
                await Task.Delay(500);
                
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
                    m_NotifLogHolder.AddLog((string) cardObject["name"] + " found");
                }
                else
                {
                    response.StatusCode.Log("Status Code");
                }
            }

            foreach (JObject cardObject in cardObjects)
            {
                ApiCardData cardData = await cardObject.ToPreviewCardData();
                cardData.m_CardSaveName.Log("Save");
                CardFileHelper.DownloadToLibrary(cardData,m_BorderColor,m_BorderSize);
            }
        }
    }
}