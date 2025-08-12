using System;
using System.Collections.Generic;
using System.IO;
using Script.Helper;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Manager
{
    public class DeckGestionController : MonoBehaviour
    {
        [SerializeField] private CardInLibraryPointer[] m_CardDisplayer = null;
        [SerializeField] private TMP_Text m_CardPageCount = null;
        [SerializeField] private TMP_InputField m_FilterInput = null;

        [Header("Card in Deck")] 
        [SerializeField] private Transform m_InDeckLayout = null;
        [SerializeField] private CardInDeckHolder m_CardInDeckUIHolder = null;

        private Dictionary<string, Sprite> m_CardsSprite = new Dictionary<string, Sprite>();
        private List<CardNameData> m_CurrentCardsToDisplay = null;
        
        private List<CardNameData> m_CardsInLibrary = null;

        private const int CARD_COUNT_DISPLAY = 20;
        private int m_CurrentPage = 0;
        private int m_CurrentMaxPage = 0;
        private DeckData m_CurrentDeckData;

        private Dictionary<string,CardInDeckHolder> m_CurrentCardInDeck = new Dictionary<string,CardInDeckHolder>();

        private void Awake()
        {
            m_FilterInput.onValueChanged.AddListener(ApplyNameFilter);
        }

        private void OnEnable()
        {
            m_CurrentPage = 0;
            FetchCardsInLibrary();
            m_CurrentCardsToDisplay = new List<CardNameData>(m_CardsInLibrary);
            GenerateCardsSprite();
            DisplayCards();
            UpdateMaxPageCount();
            UpdatePageUI();
            ClearInDeckCards();
            DisplayCurrentDeckCards();
        }

        private void UpdateMaxPageCount()
        {
            m_CurrentMaxPage = Math.Max(0,(int)Mathf.Floor(((float)m_CurrentCardsToDisplay.Count - 1)  / CARD_COUNT_DISPLAY));
            UpdatePageUI();
        }

        private void UpdatePageUI()
        {
            m_CardPageCount.text = (m_CurrentPage+1) + "/" + (m_CurrentMaxPage+1);
        }

        public void NextPage()
        {
            if(m_CurrentPage == m_CurrentMaxPage)
                return;

            m_CurrentPage++;
            DisplayCards();
            UpdatePageUI();
        }

        public void PreviousPage()
        {
            if(m_CurrentPage == 0)
                return;

            m_CurrentPage--;
            DisplayCards();
            UpdatePageUI();
        }

        private void DisplayCards()
        {
            int startIndex = m_CurrentPage * CARD_COUNT_DISPLAY;
            //Replace by CardInCurrentResearchFilter//
            int endIndex = Math.Min(m_CurrentCardsToDisplay.Count, startIndex + CARD_COUNT_DISPLAY);

            int y = 0;
            for (int i = startIndex; i < endIndex; i++,y++)
            {
                m_CardDisplayer[y].gameObject.SetActive(true);

                m_CardsSprite.TryGetValue(m_CurrentCardsToDisplay[i].CardId, out Sprite cardSprite);
                m_CardDisplayer[y].Initialize(m_CurrentCardsToDisplay[i].CardId,cardSprite);
            }

            for (; y < CARD_COUNT_DISPLAY; y++)
            {
                m_CardDisplayer[y].gameObject.SetActive(false);
            }
        }
        
        private void ApplyNameFilter(string filter)
        {
            m_CurrentCardsToDisplay.Clear();

            for (int i = 0; i < m_CardsInLibrary.Count; i++)
            {
                if (m_CardsInLibrary[i].CardName.Contains(filter,StringComparison.OrdinalIgnoreCase))
                {
                    m_CurrentCardsToDisplay.Add(m_CardsInLibrary[i]);
                }
            }

            m_CurrentPage = 0;
            UpdateMaxPageCount();
            DisplayCards();
        }
        
        private void FetchCardsInLibrary()
        {
            CardFileHelper.UpdateLibrary();
            m_CardsInLibrary = CardFileHelper.GetCardsInLibrary();
        }

        private void GenerateCardsSprite()
        {
            for (int i = 0; i < m_CardsInLibrary.Count; i++)
            {
                if(m_CardsSprite.ContainsKey(m_CardsInLibrary[i].CardId))
                    continue;
                
                m_CardsSprite.Add(m_CardsInLibrary[i].CardId,m_CardsInLibrary[i].CardPathName.ToCardSprite());
            }
        }

        public void SetDeckData(DeckData deckData)
        {
            m_CurrentDeckData = deckData;
        }

        private void ClearInDeckCards()
        {
            foreach (CardInDeckHolder cardInDeckHolder in m_CurrentCardInDeck.Values)
            {
                Destroy(cardInDeckHolder.gameObject);
            }
            
            m_CurrentCardInDeck.Clear();
        }
        private void DisplayCurrentDeckCards()
        {
            for (int i = 0; i < m_CurrentDeckData.DeckCards.Count; i++)
            {
                AddCardDisplayer(m_CurrentDeckData.DeckCards[i].CardId,m_CurrentDeckData.DeckCards[i].Count);
            }
        }

        private void AddCardDisplayer(string id, int count)
        {
            if (m_CardsSprite.TryGetValue(id, out Sprite sprite))
            {
                CardInDeckHolder cardInDeck = Instantiate(m_CardInDeckUIHolder, m_InDeckLayout);
                cardInDeck.Initialize(sprite,count,id,this);
                m_CurrentCardInDeck.Add(id,cardInDeck);
            }
        }

        public int ChangeCardCount(string cardId, int currentCount, bool updateDisplayer = false)
        {
            for (int i = 0; i < m_CurrentDeckData.DeckCards.Count; i++)
            {
                CardCount cardCount = m_CurrentDeckData.DeckCards[i];
                if (cardCount.CardId == cardId)
                {
                    cardCount.Count += currentCount;
                    
                    if (cardCount.Count == 0)
                    {
                        m_CurrentDeckData.DeckCards.RemoveAt(i);
                        RemoveCardDisplayer(cardId);
                        return cardCount.Count;
                    }

                    if (updateDisplayer)
                    {
                        m_CurrentCardInDeck.TryGetValue(cardId, out CardInDeckHolder cardInDeck);
                        if (cardInDeck != null) cardInDeck.UpdateCoutUI(cardCount.Count);
                    }
                    m_CurrentDeckData.DeckCards[i] = cardCount;
                    return cardCount.Count;
                }
            }

            m_CurrentDeckData.DeckCards.Add(new CardCount(1,cardId));
            AddCardDisplayer(cardId,currentCount);
            return 0;
        }

        private void RemoveCardDisplayer(string id)
        {
            if (m_CurrentCardInDeck.TryGetValue(id, out CardInDeckHolder card))
            {
                Destroy(card.gameObject);
                m_CurrentCardInDeck.Remove(id);
            }
        }

        public void SaveDeck()
        {
            string[] deckData = m_CurrentDeckData.ToFile();
            File.WriteAllLines(CardFileHelper.GetDeckPath() + m_CurrentDeckData.DeckName + ".dck",deckData);
        }
    }

    public struct CardNameData
    {
        public string CardPathName;
        public string CardFileName;
        public string CardName;
        public string CardId;

        public CardNameData(string cardName, string cardId,string cardFileName,string cardPathName)
        {
            CardName = cardName;
            CardId = cardId;
            CardFileName = cardFileName;
            CardPathName = cardPathName;
        }
    }
}