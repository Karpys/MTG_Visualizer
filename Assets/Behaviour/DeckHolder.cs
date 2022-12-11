using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace MTG
{
    public class DeckHolder:Holder
    {
        [SerializeField] private DeckScriptable m_Deck;

        [SerializeField] private List<CardHolder> m_CardsInDeck = new List<CardHolder>();

        public DeckScriptable Deck => m_Deck;
        private void Start()
        {
            m_Deck.m_Cards.Shuffle();
            
            for (int i = 0; i < m_Deck.m_Cards.Count; i++)
            {
                CardHolder card = Instantiate(Library.Instance.m_CardHolder, transform.position, Quaternion.identity,transform);
                card.Initialize(m_Deck.m_Cards[i]);
                AddCard(card);
            }
        }

        protected override void UpdateCardPosition(Transform card, int index)
        {
            return;
        }

        protected override Vector3 GetPosition(int index)
        {
            return Vector3.zero;
        }
    }
}