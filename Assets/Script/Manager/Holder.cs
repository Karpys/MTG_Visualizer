using System.Collections.Generic;
using DG.Tweening;
using MTG;
using UnityEngine;

namespace MTG
{
    public abstract class Holder:MonoBehaviour
    {
        [SerializeField] protected Transform m_StartTransform;
        [SerializeField] protected Transform m_EndTransform;
        [SerializeField] protected float m_HorizontalSpacing = 1f;
        [SerializeField] protected float m_VerticalSpacing = 1f;
        [SerializeField] protected List<CardHolder> m_Cards = new List<CardHolder>();

        public List<CardHolder> Cards => m_Cards;
        public virtual void AddCard(CardHolder card)
        {
            m_Cards.Add(card);
            UpdateCardPosition(card,m_Cards.Count - 1);
            card.transform.parent = transform;
        }

        public virtual void RemoveCard(CardHolder card)
        {
            m_Cards.Remove(card);
            UpdateCardsPosition();
        }
        
        private void UpdateCardsPosition()
        {
            for (int i = 0; i < m_Cards.Count; i++)
            {
                UpdateCardPosition(m_Cards[i], i);
            }
        }

        protected virtual void UpdateCardPosition(CardHolder card, int index)
        {
            card.transform.DOMove(GetPosition(card,index), 1f);
        }

        protected virtual Vector3 GetPosition(CardHolder card, int index)
        {
            Vector3 newPos = m_StartTransform.position;
            int y = 0;
            for (int i = 0; i < index; i++)
            {
                newPos.x += m_HorizontalSpacing;
                if (newPos.x > m_EndTransform.position.x)
                {
                    newPos.x = m_StartTransform.position.x;
                    y++;
                    newPos.y -= m_VerticalSpacing;
                }
            }
            
            card.SetSpritePriority(y);
            
            return newPos;
        }
    }
}