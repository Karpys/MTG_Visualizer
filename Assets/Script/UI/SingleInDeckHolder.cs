namespace Script.UI
{
    using System;
    using Manager;
    using UnityEngine;
    using UnityEngine.UI;

    public class SingleInDeckHolder : BaseCardInDeckUI
    {
        private DeckGestionContext m_Context = DeckGestionContext.Token;
        
        public void Initialize(DeckGestionContext context)
        {
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