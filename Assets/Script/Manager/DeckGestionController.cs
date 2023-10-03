using System;
using System.Collections.Generic;
using System.IO;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Manager
{
    public class DeckGestionController : MonoBehaviour
    {
        [SerializeField] private Image[] m_CardDisplayer = null;
        [SerializeField] private TMP_Text m_CardPageCount = null;

        [Header("Card in Deck")] 
        [SerializeField] private Transform m_InDeckLayout = null;
        [SerializeField] private CardInDeckHolder m_CardInDeckUIHolder = null;

        private Dictionary<string, Sprite> m_CardsSprite = new Dictionary<string, Sprite>();
        private List<CardNameData> m_CardsInLibrary = null;

        private const int CARD_COUNT_DISPLAY = 20;
        private int m_CurrentPage = 0;
        private int m_CurrentMaxPage = 0;
        private DeckData m_CurrentDeckData;

        private List<CardInDeckHolder> m_CurrentInDeckCards = new List<CardInDeckHolder>();

        private void OnEnable()
        {
            m_CurrentPage = 0;
            FetchCardsInLibrary();
            GenerateCardsSprite();
            DisplayCards();
            UpdateMaxPageCount();
            UpdatePageUI();
            ClearInDeckCards();
            DisplayCurrentDeckCards();
        }

        private void UpdateMaxPageCount()
        {
            m_CurrentMaxPage = (int)Mathf.Floor((float)m_CardsInLibrary.Count / CARD_COUNT_DISPLAY);
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
            int endIndex = Math.Min(m_CardsInLibrary.Count, startIndex + CARD_COUNT_DISPLAY);

            int y = 0;
            for (int i = startIndex; i < endIndex; i++,y++)
            {
                m_CardDisplayer[y].gameObject.SetActive(true);

                m_CardsSprite.TryGetValue(m_CardsInLibrary[i].CardId, out Sprite cardSprite);
                m_CardDisplayer[y].sprite = cardSprite;
            }

            for (; y < CARD_COUNT_DISPLAY; y++)
            {
                m_CardDisplayer[y].gameObject.SetActive(false);
            }
        }
        
        private void FetchCardsInLibrary()
        {
            m_CardsInLibrary = FileHelper.GetCardsInLibrary();
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
            for (int i = 0; i < m_CurrentInDeckCards.Count; i++)
            {
                Destroy(m_CurrentInDeckCards[i].gameObject);
            }
            
            m_CurrentInDeckCards.Clear();
        }
        private void DisplayCurrentDeckCards()
        {
            for (int i = 0; i < m_CurrentDeckData.DeckCards.Count; i++)
            {
                AddCard(m_CurrentDeckData.DeckCards[i].CardId,m_CurrentDeckData.DeckCards[i].Count);
            }
        }

        private void AddCard(string id, int count)
        {
            if (m_CardsSprite.TryGetValue(id, out Sprite sprite))
            {
                CardInDeckHolder cardInDeck = Instantiate(m_CardInDeckUIHolder, m_InDeckLayout);
                cardInDeck.Initialize(sprite,count,id,this);
                m_CurrentInDeckCards.Add(cardInDeck);
            }
        }

        public int ChangeCardCount(string cardId, int currentCount)
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
                        RemoveCard(cardId);
                        return cardCount.Count;
                    }

                    m_CurrentDeckData.DeckCards[i] = cardCount;
                    return cardCount.Count;
                }
            }

            AddCard(cardId,currentCount);
            return 0;
        }

        private void RemoveCard(string id)
        {
            for (int i = 0; i < m_CurrentInDeckCards.Count; i++)
            {
                if (m_CurrentInDeckCards[i].CardId == id)
                {
                    Destroy(m_CurrentInDeckCards[i].gameObject);
                    m_CurrentInDeckCards.RemoveAt(i);
                    return;
                }
            }
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