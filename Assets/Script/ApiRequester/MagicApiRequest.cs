using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Script.Manager;
using UnityEngine;

namespace Script
{
    public class MagicApiRequest
    {
        private HttpClient m_Client = new HttpClient();
        private string m_Lang = "lang:en";
        public  Action<JObject> OnCardFound = null;
        public  Action<JObject[]> OnCardsFound = null;
        public Action<PreviewCardData,JObject> OnCardPreview = null;
        public Action OnFailCardFound = null;

        public int MAX_CARDS = 175;

        public MagicApiRequest()
        {
            m_Client.DefaultRequestHeaders.Add("User-Agent", "MonApplication/1.0");
        }

        public void SetLang(string lang)
        {
            if (lang.Length == 0)
            {
                m_Lang = "";
                return;
            }
            
            m_Lang = "lang:"+lang;
        }

        public async Task FindCard(string cardName)
        {
            string request = "https://api.scryfall.com/cards/search?q=" + m_Lang + "+!" + "\"" + cardName + "\"";
            HttpResponseMessage response = await m_Client.GetAsync(request);
            request.Log("Request");
            Debug.Log("Try Find card");
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var cardJson = JObject.FromObject(JObject.Parse(responseContent)["data"][0]);
                OnCardFound?.Invoke(cardJson);
                Debug.Log("card found");
            }
            else
            {
                OnFailCardFound?.Invoke();
            }
        }

        public async Task FindCardsArts(string cardName)
        {
            HttpResponseMessage response = await m_Client.GetAsync("https://api.scryfall.com/cards/search?q=" + m_Lang + "+!" + "\"" + cardName + "\"" + "&unique=art");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                JObject cardsJson = JObject.Parse(responseContent);
                int totalCards = (int)cardsJson["total_cards"];

                totalCards = Mathf.Min(MAX_CARDS, totalCards);
                JObject[] cards = new JObject[totalCards];
                
                for (int i = 0; i < totalCards; i++)
                {
                    cards[i] = JObject.FromObject(cardsJson["data"][i]);
                }
                
                Debug.Log("On Cards Found");
                OnCardsFound?.Invoke(cards);
            }
            else
            {
                OnFailCardFound?.Invoke();
            }
        }

        public async Task FindAbstractCards(string filter)
        {
            string request = "https://api.scryfall.com/cards/search?q=" + m_Lang + filter;
            request.Log("Request");
            HttpResponseMessage response = await m_Client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                JObject cardsJson = JObject.Parse(responseContent);
                int totalCards = (int)cardsJson["total_cards"];

                totalCards = Mathf.Min(MAX_CARDS, totalCards);
                JObject[] cards = new JObject[totalCards];
                
                for (int i = 0; i < totalCards; i++)
                {
                    cards[i] = JObject.FromObject(cardsJson["data"][i]);
                }
                
                Debug.Log("On Cards Found");
                OnCardsFound?.Invoke(cards);
            }
            else
            {
                OnFailCardFound?.Invoke();
            }
        }

        public async Task PreviewCards(List<JObject> cards, CancellationTokenSource cancellationTokenSource)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
                else
                {
                    await PreviewCard(cards[i],cancellationTokenSource);
                }
            }
        }

        public async Task PreviewCard(JObject cardObject,CancellationTokenSource cancellationTokenSource)
        {
            PreviewCardData previewCardData = await cardObject.ToPreviewCardData();
            if(cancellationTokenSource is {IsCancellationRequested:true})
                return;
            OnCardPreview?.Invoke(previewCardData,cardObject);
        }
    }


    public struct PreviewCardData
    {
        public string cardSaveName;
        public string cardName;
        public string cardId;
        public Sprite sprite;
    }
}