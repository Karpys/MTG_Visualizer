namespace Script.UI
{
    using System;
    using Helper;
    using Manager;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class BaseCardInDeckUI : MonoBehaviour , IPosition
    {
        [SerializeField] protected Image m_CardSprite = null;
        
        protected CardDisplayData m_CardDisplayData;
        protected string m_CardId = String.Empty;
        protected DeckGestionController m_Controller = null;
        
        public Vector3 Position => transform.position;
        public CardDisplayData DisplayData => m_CardDisplayData;
        public string Id => m_CardId;
        public Sprite CardSprite => m_CardSprite.sprite;

        public void InitializeBaseCard(CardDisplayData cardDisplayData, string cardId,
            DeckGestionController gestionController)
        {
            m_CardDisplayData = cardDisplayData;
            m_CardSprite.sprite = cardDisplayData.m_FrontSprite;
            m_CardId = cardId;
            m_Controller = gestionController;
        }
    }
}