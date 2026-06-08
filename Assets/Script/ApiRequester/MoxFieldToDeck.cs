namespace Script
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Helper;
    using KarpysDev.KarpysUtils;
    using UI;
    using Unity.Plastic.Newtonsoft.Json.Linq;
    using UnityEngine;

    public class MoxFieldToDeck : MonoBehaviour
    {
        [SerializeField] private NotifLogHolder m_NotifHolder = null;
        
        private HttpClient m_Client = new HttpClient();
        
        private void Awake()
        {
            m_Client.DefaultRequestHeaders.Add("User-Agent", "MonApplication/1.0");
            m_Client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public void OpenFileSelection()
        {
            string filePath = FileHelper.GetFilePath(FilterType.Text);
            
            if (filePath != String.Empty)
            {
                _ = MoxFieldFileToDeck(File.ReadAllLines(filePath),filePath.ToFileName());
            }
            else
            {
                Debug.LogError("Select a text file format");
            }
        }

        private async Task MoxFieldFileToDeck(string[] fileData,string fileName)
        {
            try
            {
                List<string> ids = new List<string>();
                
                foreach (string card in fileData)
                {
                    string[] cardSplit = card.Split();
                    string cardName = "";

                    for (int i = 0; i < cardSplit.Length; i++)
                    {
                        if(i == 0)
                            continue;
                        if(i == cardSplit.Length - 2)
                            break;
                        cardName += cardSplit[i] + " ";
                    }

                    string request = "https://api.scryfall.com/cards/search?q=!" +  "\"" + cardName + "\"";
                    HttpResponseMessage response = await m_Client.GetAsync(request);
                
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        JObject cardObject = JObject.Parse(responseContent);
                        string id = cardObject["data"][0]["id"].ToString();

                        if (ids.Contains(id))
                        {
                            m_NotifHolder.AddLog("Duplicate found : " + cardName, 1.5f);
                        }
                        else
                        {
                            ids.Add(id);
                            m_NotifHolder.AddLog("Card Found : " + cardName);
                        }
                    }
                    else
                    {
                        m_NotifHolder.AddLog("Card Not Found : " + cardName);
                    }

                    await Task.Delay(550);
                }

                CreateDeckWithIds(fileName,ids);
            }
            catch (TaskCanceledException)
            {
                m_NotifHolder.AddLog("NetWork error", 1.5f);
                throw;
            }
        }

        private void CreateDeckWithIds(string fileName, List<string> ids)
        {
            string deckName = fileName.Replace(".txt", "");
            DeckHelper.CreateDeckFile(deckName,1,"");
            //Fetch deck and add each card in his cards list
            fileName = DeckHelper.DeckNameToDeckFile(deckName);
            string[] deckInfo = File.ReadAllLines(fileName);
            DeckData deckData = deckInfo.ToDeckData();

            List<CardCount> cardCounts = new List<CardCount>();
            
            foreach (string id in ids)
            {
                cardCounts.Add(new CardCount(1,id));    
            }

            deckData.DeckCards = cardCounts;
            deckData.SaveDeck();
        }
    }
}