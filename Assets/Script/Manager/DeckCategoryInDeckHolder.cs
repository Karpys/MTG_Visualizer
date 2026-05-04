namespace Script.Manager
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class DeckCategoryInDeckHolder : MonoBehaviour
    {
        [SerializeField] private RectTransform m_Rect = null;
        [SerializeField] private TMP_Text m_CategoryName = null;
        
        private Vector2Int m_Range = Vector2Int.zero;

        private static int CARD_PER_ROW = 8;
        public void Initialize(Vector2Int range, string name, GridLayoutGroup gridLayout)
        {
            m_CategoryName.text = name;
            m_Range = range;
         
            Vector2 anchoredPosition = Vector2.zero;

            int verticalPlace = (int)Mathf.Floor(range.x / (float)CARD_PER_ROW);
            anchoredPosition.y = -20 - verticalPlace * gridLayout.cellSize.y - verticalPlace * gridLayout.spacing.y;
            anchoredPosition.x = 16 + range.x * gridLayout.cellSize.x + range.x * gridLayout.spacing.x;
            m_Rect.anchoredPosition = anchoredPosition;
        }
    }
}