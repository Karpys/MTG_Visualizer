using System;
using Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class CardInDeckHolder : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_CardCount = null;
        [SerializeField] private Image m_CardSprite = null;

        private CardDisplayData m_CardDisplayData;
        private string m_CardId = String.Empty;
        private DeckGestionController m_Controller = null;

        public CardDisplayData DisplayData => m_CardDisplayData;
        
        public void Initialize(CardDisplayData displayData, int cardCount,string cardId, DeckGestionController controller)
        {
            m_CardDisplayData = displayData;
            m_CardSprite.sprite = displayData.m_FrontSprite;
            m_CardCount.text = cardCount.ToString();
            m_CardId = cardId;
            m_Controller = controller;
        }

        public void ChangeCardCount(int count)
        {
            int newCount = m_Controller.ChangeCardCount(m_CardId, count);
            UpdateCoutUI(newCount);
        }

        public void UpdateCoutUI(int cardCountCount)
        {
            m_CardCount.text = cardCountCount.ToString();
        }
    }
}