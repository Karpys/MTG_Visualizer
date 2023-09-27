using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Manager
{
    public class DeckGestionController : MonoBehaviour
    {
        [SerializeField] private Image[] m_CardDisplayer = null;
        [SerializeField] private TMP_Text m_CardPageCount = null;
        
        private Dictionary<string, Sprite> m_CardsSprite = new Dictionary<string, Sprite>();
        private List<CardNameData> m_CardsInLibrary = null;

        private const int CARD_COUNT_DISPLAY = 20;
        private int m_CurrentPage = 0;
        private int m_CurrentMaxPage = 0;
        private void OnEnable()
        {
            m_CurrentPage = 0;
            FetchCardsInLibrary();
            GenerateCardsSprite();
            DisplayCards();
            UpdateMaxPageCount();
            UpdatePageUI();
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