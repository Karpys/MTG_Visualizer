using System;
using UnityEngine;
using UnityEngine.UI;

public class CardGridSetter : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup m_CardGrid = null;
    [SerializeField] private RectTransform m_RectTransform = null;

    private int m_CardCount = 0;
    public int CardCount => m_CardCount;
    
    private void Update()
    {
        Vector2Int gridSize = Vector2Int.zero;
        gridSize.x = (int)Math.Floor(m_RectTransform.rect.width / m_CardGrid.cellSize.x);
        gridSize.y = (int)Math.Floor(m_RectTransform.rect.height / m_CardGrid.cellSize.y);

        Vector2 spacing = m_CardGrid.spacing;

        float totalWidth = gridSize.x * spacing.x + gridSize.x * m_CardGrid.cellSize.x + 5;
        
        if (gridSize.x > 2 && totalWidth >= m_RectTransform.rect.width)
        {
            gridSize.x -= 1;
        }
        
        m_CardCount = gridSize.x * gridSize.y;
    }
}