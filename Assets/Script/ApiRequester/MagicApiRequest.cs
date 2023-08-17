using System;
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
        public Action<string> OnCardDownload = null;


        public async Task FindCard(string cardName)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.scryfall.com/cards/named?exact=" + cardName);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var cardJson = JObject.Parse(responseContent);
                Debug.Log(responseContent);
                OnCardFound?.Invoke(cardJson);
            }
        }

        public async Task DownloadCard(JObject cardObject)
        {
            string cardImageUris = (string) cardObject["image_uris"]["normal"];
            HttpClient client = new HttpClient();

            string cardName = (string) cardObject["name"];
            Debug.Log("Start dl");
            string cardPath = await client.DownloadFile(cardImageUris, "CardImage/",(string) cardObject["name"],"jpg");
            OnCardDownload?.Invoke(cardPath);
            Debug.Log("End dl");
        }
    }
}