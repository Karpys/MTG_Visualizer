namespace Script.UI
{
    using UnityEngine;

    public class CardInDeckDraggable : MonoBehaviour , IUIDraggable
    {
        [SerializeField] private CardInDeckHolder m_CardInDeck = null;
        [SerializeField] private RectTransform m_RectTransform = null;
        [SerializeField] private CardShadow m_CardShadowPrefab = null;
        public RectTransform RectTransform => m_RectTransform;

        private CardShadow m_CardShadowCreated = null;
        public void OnDragUpdate(Vector2 screenPosition)
        {
            m_CardShadowCreated.transform.position = screenPosition;
        }

        public void OnDragSelect()
        {
            m_CardShadowCreated = Instantiate(m_CardShadowPrefab, transform.parent.parent);
            m_CardShadowCreated.Initialize(m_CardInDeck.CardSprite);
        }

        public void ReleaseDrag()
        {
            if (m_CardShadowCreated)
                Destroy(m_CardShadowCreated.gameObject);
        }
    }
}