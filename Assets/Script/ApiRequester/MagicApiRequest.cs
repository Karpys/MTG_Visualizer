using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Script
{
    public class MagicApiRequest
    {
        private string m_Lang = "";
        public  Action<JObject> OnCardFound = null;
        public  Action<JObject[]> OnCardsFound = null;
        public Action<PreviewCardData> OnCardPreview = null;
        public Action OnFailCardFound = null;

        public int MAX_CARDS = 175;

        public void SetLang(string lang)
        {
            if (lang.Length == 0)
            {
                m_Lang = "";
                return;
            }
            
            m_Lang = "lang:"+lang+"+";
        }

        public async Task FindCard(string cardName)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.scryfall.com/cards/search?q=" + m_Lang + "!" + "\"" + cardName + "\"");

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
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.scryfall.com/cards/search?q=" + m_Lang + "!" + "\"" + cardName + "\"" + "&unique=art");

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

        public async Task FindAbstractCards(string cardName)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.scryfall.com/cards/search?q=" + m_Lang + "name:" + cardName);

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

        //Obsolete//
        public async Task DownloadCard(JObject cardObject,string location,string fallBackLocation,CancellationTokenSource cancellationTokenSource = null)
        {
            string cardImageUris = String.Empty;
            
            if (cardObject["image_uris"] == null)
            {
                if (cardObject["card_faces"] != null)
                {
                    cardImageUris = (string)cardObject["card_faces"][0]["image_uris"]["normal"];
                }
                else
                {
                    Debug.LogError("Unknown cards");
                }
            }
            else
            {
                cardImageUris = (string)cardObject["image_uris"]["normal"];
            }
        
            HttpClient client = new HttpClient();

            string cardName = (string) cardObject["name"];
            cardName = cardName.Replace("/", "");
            string cardSaveName = cardName + "~" + cardObject["id"];
            string cardPath = await client.DownloadFile(cardImageUris, location,cardSaveName,"jpg",fallBackLocation);
        
            PreviewCardData data = new PreviewCardData();
            //data.cardPath = cardPath;
            //data.cardId = (string)cardObject["id"];
            data.cardSaveName = cardSaveName;
        
            
            if(cancellationTokenSource is {IsCancellationRequested:true})
                return;
            OnCardPreview?.Invoke(data);
        }

        //Obsolete//
        public async Task DownloadCards(List<JObject> cards, string location,string fallBackLocation,CancellationTokenSource cancellationTokenSource)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }
                else
                {
                    await DownloadCard(cards[i], location,fallBackLocation,cancellationTokenSource);
                }
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
            string cardImageUris = String.Empty;

            if (cardObject["image_uris"] == null)
            {
                if (cardObject["card_faces"] != null)
                {
                    cardImageUris = (string)cardObject["card_faces"][0]["image_uris"]["normal"];
                }
                else
                {
                    Debug.LogError("Unknown cards");
                }
            }
            else
            {
                cardImageUris = (string)cardObject["image_uris"]["normal"];
            }
            
            HttpClient client = new HttpClient();

            string cardName = (string) cardObject["name"];
            cardName = cardName.Replace("/", "");
            string cardSaveName = cardName + "~" + cardObject["id"];
            
            PreviewCardData data = new PreviewCardData();
            data.cardSaveName = cardSaveName;
            byte[] cardImageBytes = await client.ReadFile(cardImageUris);
            data.sprite = cardImageBytes.ToCardSprite();
            
            if(cancellationTokenSource is {IsCancellationRequested:true})
                return;
            OnCardPreview?.Invoke(data);
        }
    }


    public struct PreviewCardData
    {
        public string cardSaveName;
        public Sprite sprite;
    }
}