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

        private string m_CurrentCardId = String.Empty;
        public void Initialize(string cardId,Sprite sprite)
        {
            m_CurrentCardId = cardId;
            m_Image.sprite = sprite;
        }
        protected override void OnExit()
        {
            return;
        }

        public override void OnRightClick()
        {
            GlobalCanvas.Instance.DisplayCardViewer(m_Image.sprite);
        }

        public override void OnLeftClick()
        {
            m_Controller.ChangeCardCount(m_CurrentCardId, 1,true);
        }
    }
}