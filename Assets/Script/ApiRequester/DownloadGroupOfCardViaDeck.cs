namespace Script
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Helper;
    using KarpysDev.KarpysUtils;
    using Newtonsoft.Json.Linq;
    using UnityEngine;

    public class DownloadGroupOfCardViaDeck : MonoBehaviour
    {
        [SerializeField] private Color m_BorderColor = Color.black;
        [SerializeField] private Vector2Int m_BorderSize = new Vector2Int(10,10);
        [SerializeField] private NotifLogHolder m_NotifLogHolder = null;

        private HttpClient m_Client = new HttpClient();

        private void Awake()
        {
            m_Client.DefaultRequestHeaders.Add("User-Agent", "MonApplication/1.0");
            m_Client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public void OpenFileSelection()
        {
            string filePath = FileHelper.GetFilePath(FilterType.Deck);
            
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
            List<JObject> cardObjects = new List<JObject>();

            for (int i = 0; i < cards.Length; i++)
            {
                Debug.Log("Try found " + cards[i]);
                if(!cards[i].Contains('|'))
                    continue;
                
                
                string cardId = cards[i].Split('|')[1];
                Debug.Log("Search " + cardId);
                string request = "https://api.scryfall.com/cards/" + cardId;
                HttpResponseMessage response = await m_Client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    responseContent.Log("Response");
                    JObject cardObject = JObject.FromObject(JObject.Parse(responseContent));
                    cardObjects.Add(cardObject);
                    m_NotifLogHolder.AddLog((string) cardObject["name"]);
                    cardId.Log("Found");
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