namespace Script.UI
{
    using System.IO;
    using Helper;
    using TMPro;
    using UnityEngine;

    public class ApiResearchDisplayCardViewer : DisplayCardViewer
    {
        [SerializeField] private Vector2Int m_BorderSize = new Vector2Int(10, 10);
        [SerializeField] private Color m_BorderColor = Color.black;
        [SerializeField] private TMP_InputField m_DeckNameInputField = null;
        [SerializeField] private TMP_Text m_ResultDeckFound = null;
        
        private ApiCardData m_CardData;

        public void DisplayApiCard(ApiCardData apiCardData)
        {
            m_CardData = apiCardData;
            Display(apiCardData.m_FrontCardSprite,apiCardData.m_BackCardSprite);
            m_ResultDeckFound.text = "";
        }
        
        public void DownloadCardAndInsertInsideDeck()
        {
            string deckName = m_DeckNameInputField.text;

            if (!DeckExist(deckName))
            {
                m_ResultDeckFound.text = "Deck not found";
                return;
            }
            else
            {
                m_ResultDeckFound.text = "Deck found, card save";
            }
            
            DownloadCard();
            string deckFilePath = CardFileHelper.GetDeckPath() + deckName + ".deck";
            string[] deckLines = File.ReadAllLines(deckFilePath);

            DeckData deckData = deckLines.ToDeckData();

            bool wasAlreadyInDeck = false;
            for (int i = 0; i < deckData.DeckCards.Count; i++)
            {
                if (deckData.DeckCards[i].CardId == m_CardData.m_CardId)
                {
                    wasAlreadyInDeck = true;
                    deckData.DeckCards[i] = new CardCount(deckData.DeckCards[i].Count + 1, m_CardData.m_CardId);
                    break;
                }
            }
            
            if(!wasAlreadyInDeck)
                deckData.DeckCards.Add(new CardCount(1,m_CardData.m_CardId));
            
            deckLines = deckData.ToFile();
            File.WriteAllLines(CardFileHelper.GetDeckPath() + deckName + ".deck",deckLines);
        }

        private bool DeckExist(string deckName)
        {
            return File.Exists(CardFileHelper.GetDeckPath() + deckName + ".deck");
        }

        private void DownloadCard()
        {
            CardFileHelper.DownloadToLibrary(m_CardData,m_BorderColor,m_BorderSize);
        }
    }
}