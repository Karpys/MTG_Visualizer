using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Ookii.Dialogs;
using Script.Manager;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

namespace Script
{
    public class DownloadGroupOfCard : MonoBehaviour
    {
        [SerializeField] private Color m_BorderColor = Color.black;
        [SerializeField] private Vector2Int m_BorderSize = new Vector2Int(10,10);

        public void OpenFileSelection()
        {
            VistaOpenFileDialog openFile = new VistaOpenFileDialog();
            openFile.Filter = "Text (*.txt)|*.txt";
            
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                DownloadCards(File.ReadAllLines(openFile.FileName));
            }
            else
            {
                Debug.LogError("Select a correct file format");
            }
        }
        
        private async Task DownloadCards(string[] cards)
        {
            HttpClient client = new HttpClient();
            List<JObject> cardObjects = new List<JObject>();
            
            for (int i = 0; i < cards.Length; i++)
            {
                string request = "https://api.scryfall.com/cards/search?q=lang:any+!" + "\"" + cards[i] + "\"";
                HttpResponseMessage response = await client.GetAsync(request);
                
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var cardObject = JObject.FromObject(JObject.Parse(responseContent)["data"][0]);
                    cardObjects.Add(cardObject);
                }
            }

            foreach (JObject cardObject in cardObjects)
            {
                PreviewCardData cardData = await cardObject.ToPreviewCardData();
                //Todo : Add notif log
                cardData.cardSaveName.Log("Save");
                CardFileHelper.DownloadToLibrary(cardData.cardSaveName,cardData.sprite,m_BorderColor,m_BorderSize);
            }
        }
    }
}