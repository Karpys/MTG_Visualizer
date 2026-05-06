namespace Script.Manager
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class DeckCategoryInDeckHolderGroup : MonoBehaviour
    {
        [SerializeField] private DeckCategoryHolder m_Holder = null;
        [SerializeField] private DeckCategoryHolder m_OverflowHolder = null;
        
        private Vector2Int m_Range = Vector2Int.zero;
        private string m_Name = String.Empty;
        private GridLayoutGroup m_GridLayoutGroup = null;
        private RectTransform m_RectMask = null;
        private List<DeckCategoryHolder> m_DeckCategoryHolders = new List<DeckCategoryHolder>();

        private static int CARD_PER_ROW = 8;

        public void BaseInitialize(Vector2Int range, string name, GridLayoutGroup gridLayout, RectTransform rectMask)
        {
            m_Name = name;
            m_Range = range;
            m_GridLayoutGroup = gridLayout;
            m_RectMask = rectMask;
            UpdateVisual();
        }
        
        private void UpdateVisual()
        {
            Clean();
            List<DeckCategoryData> deckCategoryDatas = new List<DeckCategoryData>();
            
            int x = m_Range.x % CARD_PER_ROW;
            int y = (int)Mathf.Floor(m_Range.x / (float)CARD_PER_ROW);
            
            float categoryWidth = 0;

            for (int i = m_Range.x; i < m_Range.y + 1; i++)
            {
                if (i % CARD_PER_ROW == 0 && i != m_Range.x)
                {
                    deckCategoryDatas.Add(new DeckCategoryData(categoryWidth,GetAnchoredPosition(x,y,m_GridLayoutGroup)));
                    x = 0;
                    y += 1;
                    categoryWidth = 0;
                }
                
                categoryWidth += 16 + m_GridLayoutGroup.cellSize.x;
            }
            
            deckCategoryDatas.Add(new DeckCategoryData(categoryWidth,GetAnchoredPosition(x,y,m_GridLayoutGroup)));

            for (int i = 0; i < deckCategoryDatas.Count; i++)
            {
                var deckCategoryData = deckCategoryDatas[i];
                DeckCategoryHolder holder = null;

                if (i == 0)
                {
                    holder = Instantiate(m_Holder, transform);
                }
                else
                {
                    holder = Instantiate(m_OverflowHolder, transform);
                }

                holder.Initialize(m_Name, m_RectMask, deckCategoryData.Width, deckCategoryData.AnchoredPosition, this, i == deckCategoryDatas.Count - 1);
                m_DeckCategoryHolders.Add(holder);
            }
        }

        private void Clean()
        {
            foreach (DeckCategoryHolder holder in m_DeckCategoryHolders)
            {
                Destroy(holder.gameObject);
            }
            
            m_DeckCategoryHolders.Clear();
        }

        private Vector2 GetAnchoredPosition(float x, float y, GridLayoutGroup grid)
        {
            Vector2 anchoredPosition;
            anchoredPosition.y = -20 - y * grid.cellSize.y - y * grid.spacing.y;
            anchoredPosition.x = 16 + x * grid.cellSize.x + x * grid.spacing.x;
            return anchoredPosition;
        }

        public void TryShrink()
        {
            m_Range.y -= 1;
            UpdateVisual();
        }

        public void TryExpand()
        {
            m_Range.y += 1;
            UpdateVisual();
        }
    }
}