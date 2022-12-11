using System;
using System.Collections.Generic;
using MTG;
using UnityEngine;

namespace MTG
{
    public class CardUIDisplay : SingletonMonoBehaviour<CardUIDisplay>
    {
        [SerializeField] private Transform m_CardsGroup = null;
        [SerializeField] private UICardSelection m_CardUI = null;
        [SerializeField] private RectTransform m_Display = null;

        public List<UICardSelection> UICards = new List<UICardSelection>();

        public bool InDisplay = false;

        public void UpdateDisplay(List<CardHolder> holders, CardState zone)
        {
            ClearDisplay();
            UICards.Clear();
            for (int i = 0; i < holders.Count; i++)
            {
                UICardSelection cardUI = Instantiate(m_CardUI, m_CardsGroup);
                cardUI.Initialize(i,zone,holders[i].CardVisual);
            }
        }
        public void DisplayCard(List<CardHolder> holders,CardState zone)
        {
            m_Display.gameObject.SetActive(true);
            InDisplay = true;
            UpdateDisplay(holders,zone);
        }

        private void ClearDisplay()
        {
            for (int i = 0; i < m_CardsGroup.childCount; i++)
            {
                Destroy(m_CardsGroup.GetChild(i).gameObject);
            }
        }

        public void CloseDisplay()
        {
            ClearDisplay();
            InDisplay = false;
            m_Display.gameObject.SetActive(false);
        }
    }
}