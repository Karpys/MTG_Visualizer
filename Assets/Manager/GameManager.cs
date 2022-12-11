using System;
using UnityEditor.U2D.Path;
using UnityEngine;

namespace MTG
{
    public class GameManager:SingletonMonoBehaviour<GameManager>
    {
        [SerializeField] private DeckHolder m_DeckHolder = null;
        [SerializeField] private HandHolder m_HandHolder = null;

        public DeckHolder DeckHolder => m_DeckHolder;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Draw();
            }
        }

        private void Draw()
        {
            CardHolder card = m_DeckHolder.Cards[0];
            m_DeckHolder.RemoveCard(card);
            m_HandHolder.AddCard(card);
            card.UpdateState(CardState.Hand);
        }
    }
}