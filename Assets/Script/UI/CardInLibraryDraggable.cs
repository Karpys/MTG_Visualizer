namespace Script.UI
{
    using UnityEngine;

    public class CardInLibraryDraggable : MonoBehaviour, IUIDraggable
    {
        [SerializeField] private RectTransform m_RectTransform = null;
        [SerializeField] private CardInLibraryPointer m_CardInLibraryPointer = null;
        [SerializeField] private CardShadow m_CardShadowPrefab = null;
        public RectTransform RectTransform => m_RectTransform;

        private CardShadow m_CardShadowCreated = null;
        public void OnDragUpdate(Vector2 screenPosition)
        {
            m_CardShadowCreated.transform.position = screenPosition;
        }

        public void OnDragSelect()
        {
            m_CardShadowCreated = Instantiate(m_CardShadowPrefab, transform);
            m_CardShadowCreated.Initialize(m_CardInLibraryPointer.CardSprite,m_CardInLibraryPointer);
        }

        public void ReleaseDrag()
        {
            if (m_CardShadowCreated)
            {
                //Check if rect is inside deck rect

                if (RectTransformUtility.RectangleContainsScreenPoint(m_CardInLibraryPointer.Controller.DeckRect,
                        Input.mousePosition))
                {
                    m_CardInLibraryPointer.AddInDeck();
                }
                
                Destroy(m_CardShadowCreated.gameObject);
            }
        }
    }
}