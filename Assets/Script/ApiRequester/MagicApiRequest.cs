using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Script
{
    public class MagicApiRequest
    {
        public  Action<JObject> OnCardFound = null;
        public  Action<JObject[]> OnCardsFound = null;
        public Action<CardData> OnCardDownload = null;
        public Action OnFailCardFound = null;

        public int MAX_CARDS = 175;

        public async Task FindCard(string cardName)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.scryfall.com/cards/named?exact=" + cardName);

            Debug.Log("Try Find card");
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var cardJson = JObject.Parse(responseContent);
                OnCardFound?.Invoke(cardJson);
                Debug.Log("card found");
            }
            else
            {
                OnFailCardFound?.Invoke();
            }
        }

        public async Task FindCards(string cardName)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.scryfall.com/cards/search?q=!" + "\"" + cardName + "\"" + "&unique=art");

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
            HttpResponseMessage response = await client.GetAsync("https://api.scryfall.com/cards/search?q=" + cardName);

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

        public async Task DownloadCard(JObject cardObject,string location)
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
            string cardSaveName = cardName + " id~" + cardObject["id"];
            string cardPath = await client.DownloadFile(cardImageUris, location + "/",cardSaveName,"jpg");
            
            CardData data = new CardData();
            data.cardPath = cardPath;
            data.cardId = (string)cardObject["id"];
            data.cardSaveName = cardSaveName;
            
            OnCardDownload?.Invoke(data);
        }

        public async Task DownloadCards(List<JObject> cards, string location)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                await DownloadCard(cards[i], location);
            }
        }
    }


    public struct CardData
    {
        public string cardPath;
        public string cardSaveName;
        public string cardId;
        public Sprite sprite;
    }
}