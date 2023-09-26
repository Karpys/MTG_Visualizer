using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Script.Manager
{
    public class DeckGestionController : MonoBehaviour
    {
        private Dictionary<string, Sprite> m_CardsSprite = null;
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
                Debug.Log(m_CardsInLibrary[i].CardPathName);
            }
        }

        private void GenerateCardsSprite()
        {
            m_CardsSprite = new Dictionary<string, Sprite>();
            for (int i = 0; i < m_CardsInLibrary.Count; i++)
            {
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