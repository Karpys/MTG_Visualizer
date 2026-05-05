namespace Script.Manager
{
    using TMPro;
    using UI;
    using UnityEngine;

    public class DeckCategoryHolder : MonoBehaviour
    {
        [SerializeField] private RectTransform m_Rect = null;
        [SerializeField] private TMP_Text m_CategoryName = null;
        [SerializeField] private ClipRectToTarget[] m_RectToTarget = null;

        public void Initialize(string categoryName, RectTransform mask, float width, Vector2 anchoredPosition)
        {
            foreach (ClipRectToTarget clipRectToTarget in m_RectToTarget)
            {
                clipRectToTarget.Target = mask;
            }

            m_CategoryName.text = categoryName;
            m_Rect.sizeDelta = new Vector2(width, m_Rect.sizeDelta.y);
            m_Rect.anchoredPosition = anchoredPosition;
        }
    }
}