using System;
using Script.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class CardInLibraryPointer : ButtonPointer
    {
        [SerializeField] private DeckGestionController m_Controller = null;
        [SerializeField] private Image m_Image = null;
        [SerializeField] private CardInLibraryDraggable m_Draggable = null;

        private CardDisplayData m_CardDisplayData;
        private string m_CurrentCardId = String.Empty;
        public Sprite CardSprite => m_Image.sprite;
        public IUIDraggable Draggable => m_Draggable;
        public DeckGestionController Controller => m_Controller;
        public string Id => m_CurrentCardId;

        public void Initialize(string cardId,CardDisplayData cardDisplayData)
        {
            m_CurrentCardId = cardId;
            m_Image.sprite = cardDisplayData.m_FrontSprite;
            m_CardDisplayData = cardDisplayData;
        }
        protected override void OnExit()
        {
            return;
        }

        public override void OnRightClick()
        {
            GlobalCanvas.Instance.DisplayCardViewer(m_CardDisplayData.m_FrontSprite,m_CardDisplayData.m_BackSprite);
        }

        public override void OnLeftClick()
        {
            // m_Controller.AddCardInDeck(m_CurrentCardId);
        }

        public void AddInDeck()
        {
            m_Controller.AddCardInDeck(m_CurrentCardId);
        }
    }
}