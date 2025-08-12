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

        private string m_CardId = String.Empty;
        private DeckGestionController m_Controller = null;
        
        public void Initialize(Sprite cardSprite, int cardCount,string cardId, DeckGestionController controller)
        {
            m_CardSprite.sprite = cardSprite;
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