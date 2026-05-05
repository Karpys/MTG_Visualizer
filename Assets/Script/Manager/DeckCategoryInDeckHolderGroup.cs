namespace Script.Manager
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

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
    public class DeckCategoryInDeckHolderGroup : MonoBehaviour
    {
        [SerializeField] private DeckCategoryHolder m_Holder = null;
        
        private Vector2Int m_Range = Vector2Int.zero;

        private static int CARD_PER_ROW = 8;
        public void Initialize(Vector2Int range, string name, GridLayoutGroup gridLayout, RectTransform rectMask)
        {
            m_Range = range;

            List<DeckCategoryData> deckCategoryDatas = new List<DeckCategoryData>();
            
            int x = range.x % CARD_PER_ROW;
            int y = (int)Mathf.Floor(range.x / (float)CARD_PER_ROW);
            
            float categoryWidth = 0;

            for (int i = m_Range.x; i < m_Range.y + 1; i++)
            {
                if (i % CARD_PER_ROW == 0 && i != m_Range.x)
                {
                    deckCategoryDatas.Add(new DeckCategoryData(categoryWidth,GetAnchoredPosition(x,y,gridLayout)));
                    x = 0;
                    y += 1;
                    categoryWidth = 0;
                }
                
                categoryWidth += 16 + gridLayout.cellSize.x;
            }
            
            deckCategoryDatas.Add(new DeckCategoryData(categoryWidth,GetAnchoredPosition(x,y,gridLayout)));

            foreach (DeckCategoryData deckCategoryData in deckCategoryDatas)
            {
                DeckCategoryHolder holder = Instantiate(m_Holder, transform);
                holder.Initialize(name,rectMask,deckCategoryData.Width,deckCategoryData.AnchoredPosition);
            }
        }

        private Vector2 GetAnchoredPosition(float x, float y, GridLayoutGroup grid)
        {
            Vector2 anchoredPosition;
            anchoredPosition.y = -20 - y * grid.cellSize.y - y * grid.spacing.y;
            anchoredPosition.x = 16 + x * grid.cellSize.x + x * grid.spacing.x;
            return anchoredPosition;
        }
    }
}