namespace Script.Manager
{
    using System;
    using TMPro;
    using UI;
    using UnityEngine;
    using UnityEngine.UI;

    public class DeckCategoryInDeckHolder : MonoBehaviour
    {
        [SerializeField] private RectTransform m_Rect = null;
        [SerializeField] private TMP_Text m_CategoryName = null;
        [SerializeField] private ClipRectToTarget[] m_RectToTarget = null;
        
        private Vector2Int m_Range = Vector2Int.zero;

        private static int CARD_PER_ROW = 8;
        public void Initialize(Vector2Int range, string name, GridLayoutGroup gridLayout, RectTransform rectMask)
        {
            foreach (ClipRectToTarget clipRectToTarget in m_RectToTarget)
            {
                clipRectToTarget.Target = rectMask;
            }
            
            m_CategoryName.text = name;
            m_Range = range;
         
            Vector2 anchoredPosition = Vector2.zero;

            int verticalPlace = (int)Mathf.Floor(range.x / (float)CARD_PER_ROW);
            int horizontalPlace = range.x % CARD_PER_ROW;
            anchoredPosition.y = -20 - verticalPlace * gridLayout.cellSize.y - verticalPlace * gridLayout.spacing.y;
            anchoredPosition.x = 16 + horizontalPlace * gridLayout.cellSize.x + horizontalPlace * gridLayout.spacing.x;
            m_Rect.anchoredPosition = anchoredPosition;

            int categoryLengthStep = m_Range.y - m_Range.x;
            float categoryWidth = 0;
            
            for (int i = 0; i < categoryLengthStep + 1; i++)
            {
                categoryWidth += 16 + gridLayout.cellSize.x;
            }

            m_Rect.sizeDelta = new Vector2(categoryWidth, m_Rect.sizeDelta.y);
        }
    }
}