namespace Script.Manager
{
    using UnityEngine;

    public struct DeckCategoryData
    {
        private float m_width;
        private Vector2 m_AnchoredPosition;

        public float Width => m_width;
        public Vector2 AnchoredPosition => m_AnchoredPosition;
        public DeckCategoryData(float width, Vector2 anchoredPosition)
        {
            m_width = width;
            m_AnchoredPosition = anchoredPosition;
        }
    }
}