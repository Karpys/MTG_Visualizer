using System.Collections.Generic;
using MTG;
using UnityEngine;

namespace MTG
{
    public abstract class Holder:MonoBehaviour
    {
        [SerializeField] protected Transform m_StartTransform;
        [SerializeField] protected List<CardHolder> m_Cards = new List<CardHolder>();

        public List<CardHolder> Cards => m_Cards;
        public void AddCard(CardHolder card)
        {
            m_Cards.Add(card);
            UpdateCardPosition(card.transform,m_Cards.Count - 1);
        }

        public void RemoveCard(CardHolder card)
        {
            m_Cards.Remove(card);
            UpdateCardsPosition();
        }
        
        private void UpdateCardsPosition()
        {
            for (int i = 0; i < m_Cards.Count; i++)
            {
                UpdateCardPosition(m_Cards[i].transform, i);
            }
        }

        protected abstract void UpdateCardPosition(Transform card, int index);

        protected abstract Vector3 GetPosition(int index);
    }
}