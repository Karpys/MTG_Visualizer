using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Script.Manager
{
    public class DeckGestionController : MonoBehaviour
    {
        private Dictionary<CardNameData, Sprite> m_CardsSprite = null;
        private List<CardNameData> m_CardsInLibrary = null;

        public void Start()
        {
            FetchCardsInLibrary();
            GenerateCardsSprite();
        }
        
        private void FetchCardsInLibrary()
        {
            m_CardsInLibrary = FileHelper.GetCardsInLibrary();

            for (int i = 0; i < m_CardsInLibrary.Count; i++)
            {
                Debug.Log(m_CardsInLibrary[i].CardName);
                Debug.Log(m_CardsInLibrary[i].CardId);
                Debug.Log(m_CardsInLibrary[i].CardFileName);
            }
        }

        private void GenerateCardsSprite()
        {
            
        }

        
    }

    public struct CardNameData
    {
        public string CardFileName;
        public string CardName;
        public string CardId;

        public CardNameData(string cardName, string cardId,string cardFileName)
        {
            CardName = cardName;
            CardId = cardId;
            CardFileName = cardFileName;
        }
    }
}