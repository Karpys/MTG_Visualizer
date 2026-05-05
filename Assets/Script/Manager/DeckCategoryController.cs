namespace Script.Manager
{
    using KarpysDev.KarpysUtils.EditorUtils.ButtonAttribute;
    using UnityEngine;
    using UnityEngine.UI;

    public class DeckCategoryController : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup m_GridLayout = null;
        [SerializeField] private DeckCategoryInDeckHolder m_CategoryDeckHolderPrefab = null;
        [SerializeField] private Transform m_CategoryParentHolder = null;
        [SerializeField] private Vector2Int m_Debug = Vector2Int.zero;
        [SerializeField] private RectTransform m_MaskRect = null;
        
        [Button("Create Category Holder")]
        public void Initialize()
        {
            DeckCategoryInDeckHolder categoryInDeckHolder = Instantiate(m_CategoryDeckHolderPrefab, m_CategoryParentHolder);
            categoryInDeckHolder.Initialize(m_Debug,"Creature",m_GridLayout,m_MaskRect);
        }
    }
}