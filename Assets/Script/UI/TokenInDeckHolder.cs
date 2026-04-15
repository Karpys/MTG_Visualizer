namespace Script.UI
{
    using System;
    using Manager;
    using UnityEngine;
    using UnityEngine.UI;

    public class TokenInDeckHolder : MonoBehaviour
    {
        [SerializeField] private Image m_CardSprite = null;

        private CardDisplayData m_CardDisplayData;
        private string m_CardId = String.Empty;
        private DeckGestionController m_Controller = null;

        public CardDisplayData DisplayData => m_CardDisplayData;
        
        public void Initialize(CardDisplayData displayData, string cardId, DeckGestionController controller)
        {
            m_CardDisplayData = displayData;
            m_CardSprite.sprite = displayData.m_FrontSprite;
            m_CardId = cardId;
            m_Controller = controller;
        }

        public void RemoveToken()
        {
            m_Controller.RemoveToken(m_CardId);
        }

        public void DisplayToken()
        {
            GlobalCanvas.Instance.DisplayCardViewer(m_CardDisplayData.m_FrontSprite,m_CardDisplayData.m_BackSprite);
        }
    }
}