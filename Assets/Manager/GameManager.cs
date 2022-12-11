using System;
using System.Collections.Generic;
using Manager;
using UnityEditor.U2D.Path;
using UnityEngine;

namespace MTG
{
    public class GameManager:SingletonMonoBehaviour<GameManager>
    {
        [SerializeField] private DeckScriptable m_Deck = null;
        [SerializeField] private DeckHolder m_DeckHolder = null;
        [SerializeField] private HandHolder m_HandHolder = null;
        [SerializeField] private LandHolder m_LandHolder = null;
        [SerializeField] private GraveyardHolder m_GraveyardHolder = null;
        public List<CardHolder> m_CardsOnBoards = new List<CardHolder>();

        public DeckHolder DeckHolder => m_DeckHolder;
        public DeckScriptable Deck => m_Deck;
        private CardHolder m_SelectedCard = null;
        private KeyCode m_KeyPressed = KeyCode.None;

        private void Start()
        {
            m_Deck.m_Cards.Shuffle();
            for (int i = 0; i < m_Deck.m_Cards.Count; i++)
            {
                CardHolder card = Instantiate(Library.Instance.m_CardHolder, transform.position, Quaternion.identity,m_DeckHolder.transform);
                card.Initialize(m_Deck.m_Cards[i]);
                GotoCard(CardState.Deck,card);
                m_CardsOnBoards.Add(card);
            }
        }
        public void Update()
        {
            TrySelect();
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                Draw();
            }

            if(m_SelectedCard)
                FetchInput();

            if (m_SelectedCard && m_KeyPressed != KeyCode.None)
            {
                GotoCard(GetState(m_KeyPressed),m_SelectedCard);
            }
        }

        private void TrySelect()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                mousePosition.z = 0;
                
                foreach (CardHolder card in m_CardsOnBoards)
                {
                    if (card.Selection.bounds.Contains(mousePosition))
                    {
                        m_SelectedCard = card;
                    }
                }
            }
            
        }

        

        public void GotoCard(CardState state, CardHolder card)
        {
            MTG.Holder currentHolder = card.GetComponentInParent<MTG.Holder>();
            Debug.Log(currentHolder.name);
            
            card.UpdateState(state);
            currentHolder.RemoveCard(card);
            GetHolder(state).AddCard(card);

            m_SelectedCard = null;
            m_KeyPressed = KeyCode.None;
        }
        
        private void FetchInput()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                m_KeyPressed = KeyCode.H;
            }else  if (Input.GetKeyDown(KeyCode.D))
            {
                m_KeyPressed = KeyCode.D;
            }else  if (Input.GetKeyDown(KeyCode.L))
            {
                m_KeyPressed = KeyCode.L;
            }else  if (Input.GetKeyDown(KeyCode.G))
            {
                m_KeyPressed = KeyCode.G;
            } 
        }

        private CardState GetState(KeyCode inputCommand)
        {
            switch (inputCommand)
            {
                case KeyCode.H:
                    return CardState.Hand;
                case KeyCode.D:
                    return CardState.Deck;
                case KeyCode.L:
                    return CardState.Land;
                case KeyCode.G:
                    return CardState.Graveyard;
                default:
                    return CardState.Hand;
            }
        }

        private Holder GetHolder(CardState state)
        {
            switch (state)
            {
                case CardState.Deck:
                    return m_DeckHolder;
                case CardState.Hand:
                    return m_HandHolder;
                case CardState.Land:
                    return m_LandHolder;
                case CardState.Graveyard:
                    return m_GraveyardHolder;
                default:
                    return m_HandHolder;
            }
        }

        private void Draw()
        {
            GotoCard(CardState.Hand,m_DeckHolder.Cards[0]);
        }
    }
}