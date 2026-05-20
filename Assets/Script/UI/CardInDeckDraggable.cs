namespace Script.UI
{
    using Manager;
    using UnityEngine;

    public class CardInDeckDraggable : IUIDraggable
    {
        private DeckGestionController m_Controller = null;
        private RectTransform m_RectTransform = null;
        private CardShadow m_CardShadowPrefab = null;
        private Sprite m_CardSprite = null;
        private string m_CardId = null;
        public RectTransform RectTransform => m_RectTransform;

        private CardShadow m_CardShadowCreated = null;
        private int m_CountToInsert = 0;
        private bool m_HasRemove = false;

        public CardInDeckDraggable(DeckGestionController controller, string cardId,Sprite cardSprite,RectTransform target, CardShadow cardShadowPrefab)
        {
            m_Controller = controller;
            m_RectTransform = target;
            m_CardShadowPrefab = cardShadowPrefab;
            m_CardSprite = cardSprite;
            m_CardId = cardId;
        }
        
        public void OnDragUpdate(Vector2 screenPosition)
        {
            m_CardShadowCreated.transform.position = screenPosition;
        }

        public void OnDragSelect()
        {
            m_CardShadowCreated = GameObject.Instantiate(m_CardShadowPrefab, m_RectTransform.transform.parent.parent.parent.parent.parent);
            m_CardShadowCreated.Initialize(m_CardSprite);


            if (Input.GetKey(KeyCode.LeftShift))
            {
                m_CountToInsert = m_Controller.GetCardCount(m_CardId);
            }
            else
            {
                m_CountToInsert = 1;
            }
            
            m_Controller.RemoveCardCount(m_CardId,m_CountToInsert, out bool hasRemove);
            m_HasRemove = hasRemove;
        }

        public void ReleaseDrag()
        {
            if (m_CardShadowCreated)
            {
                GameObject.Destroy(m_CardShadowCreated.gameObject);
                
                if (RectTransformUtility.RectangleContainsScreenPoint(m_Controller.DeckRect,
                        Input.mousePosition))
                {
                    if (m_CountToInsert == 1)
                    {
                        m_Controller.InsertCardInDeckAt(Input.mousePosition, m_CardId);
                    }
                }
            }
        }
    }
}