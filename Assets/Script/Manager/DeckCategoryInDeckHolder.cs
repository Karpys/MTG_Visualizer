namespace Script.Manager
{
    using TMPro;
    using UnityEngine;

    public class DeckCategoryInDeckHolder : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_CategoryName = null;
        
        private Vector2Int m_Range = Vector2Int.zero;
        
        public void Initialize(Vector2Int range, string name)
        {
            m_CategoryName.text = name;
            m_Range = range;
        }
    }
}