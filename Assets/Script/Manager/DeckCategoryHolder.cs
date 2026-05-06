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
        [SerializeField] private Transform m_ExpandTransform = null;

        private DeckCategoryInDeckHolderGroup m_Group = null;

        public void Initialize(string categoryName, RectTransform mask, float width, Vector2 anchoredPosition, DeckCategoryInDeckHolderGroup group, bool isLast)
        {
            foreach (ClipRectToTarget clipRectToTarget in m_RectToTarget)
            {
                clipRectToTarget.Target = mask;
            }

            DisplayExpand(isLast);

            m_CategoryName.text = categoryName;
            m_Rect.sizeDelta = new Vector2(width, m_Rect.sizeDelta.y);
            m_Rect.anchoredPosition = anchoredPosition;
            m_Group = group;
        }

        private void DisplayExpand(bool active)
        {
            m_ExpandTransform.gameObject.SetActive(active);
        }

        public void Shrink()
        {
            m_Group.TryShrink();
        }

        public void Expand()
        {
            m_Group.TryExpand();
        }
    }
}