namespace Script.UI
{
    using System;
    using Manager;
    using UnityEngine;
    using UnityEngine.UI;

    public class SingleInDeckHolder : MonoBehaviour
    {
        [SerializeField] private Image m_CardSprite = null;
        
        private DeckGestionContext m_Context = DeckGestionContext.Token;
        private CardDisplayData m_CardDisplayData;
        private string m_CardId = String.Empty;
        private DeckGestionController m_Controller = null;

        public CardDisplayData DisplayData => m_CardDisplayData;
        
        public void Initialize(CardDisplayData displayData, string cardId, DeckGestionController controller, DeckGestionContext context)
        {
            m_CardDisplayData = displayData;
            m_CardSprite.sprite = displayData.m_FrontSprite;
            m_CardId = cardId;
            m_Controller = controller;
            m_Context = context;
        }

        public void Remove()
        {
            m_Controller.RemoveCard(m_CardId,m_Context);
        }

        public void DisplayCard()
        {
            m_Controller.DisplaySingleCard(m_CardDisplayData);
            GlobalCanvas.Instance.DisplayCardViewer(m_CardDisplayData.m_FrontSprite,m_CardDisplayData.m_BackSprite);
        }
    }
}