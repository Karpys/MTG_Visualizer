using System;
using System.Collections.Generic;
using Manager;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.UIElements;

namespace MTG
{
    public class HolderManager:SingletonMonoBehaviour<HolderManager>
    {
        [SerializeField] private DeckScriptable m_Deck = null;
        [SerializeField] private DeckHolder m_DeckHolder = null;
        [SerializeField] private HandHolder m_HandHolder = null;
        [SerializeField] private LandHolder m_LandHolder = null;
        [SerializeField] private GraveyardHolder m_GraveyardHolder = null;
        [SerializeField] private ExileHolder m_ExileHolder = null;
        [SerializeField] private CreatureHolder m_CreatureHolder = null;
        [SerializeField] private EnchantementHolder m_EnchantementHolder = null;
        public List<CardHolder> m_CardsOnBoards = new List<CardHolder>();

        private List<CardScriptable> m_CardsInDeck = new List<CardScriptable>();
        public DeckScriptable Deck => m_Deck;
        private CardHolder m_SelectedCard = null;
        private KeyCode m_KeyPressed = KeyCode.None;
        

        private void Start()
        {

            for (int i = 0; i < m_Deck.m_Cards.Count; i++)
            {
                m_CardsInDeck.Add(m_Deck.m_Cards[i]);
            }
            
            m_CardsInDeck.Shuffle();


            for (int i = 0; i < m_CardsInDeck.Count; i++)
            {
                CardHolder card = Instantiate(Library.Instance.m_CardHolder, transform.position, Quaternion.identity,m_DeckHolder.transform);
                card.Initialize(m_CardsInDeck[i]);
                GotoCard(CardState.Deck,card);
                m_CardsOnBoards.Add(card);
            }
        }
        public void Update()
        {
            m_KeyPressed = KeyCode.None;
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                Draw();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                AlternativeAction();
            }
            else
            {
                FecthAction();
            }

            if (m_SelectedCard && m_KeyPressed != KeyCode.None)
            {
                GotoCard(GetState(m_KeyPressed),m_SelectedCard);
            }
        }

        private void AlternativeAction()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                CardUIDisplay.Instance.DisplayCard(GetHolder(GetState(KeyCode.D)).Cards, GetState(KeyCode.D));
            }else  if (Input.GetKeyDown(KeyCode.G))
            {
                CardUIDisplay.Instance.DisplayCard(GetHolder(GetState(KeyCode.G)).Cards, GetState(KeyCode.G));
            }else  if (Input.GetKeyDown(KeyCode.E))
            {
                CardUIDisplay.Instance.DisplayCard(GetHolder(GetState(KeyCode.E)).Cards, GetState(KeyCode.E));
            }
        }

        private void SelectCard()
        {
            if(CardUIDisplay.Instance.InDisplay)
                return;
            m_SelectedCard = InstantSelect();
        }

        // private void TrySelect()
        // {
        //     List<CardHolder> selectable = new List<CardHolder>();
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         Vector3 mousePosition = Input.mousePosition;
        //         mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //         mousePosition.z = 0;
        //         
        //         foreach (CardHolder card in m_CardsOnBoards)
        //         {
        //             if (card.Selection.bounds.Contains(mousePosition))
        //             {
        //                 selectable.Add(card);
        //             }
        //         }
        //     }
        //
        //     if (selectable.Count == 0) return;
        //     
        //     int biggestPriority = 0;
        //     int index = 0;
        //
        //     for (int i = 0; i < selectable.Count; i++)
        //     {
        //         if (selectable[i].GetPriority() > biggestPriority)
        //         {
        //             index = i;
        //             biggestPriority = selectable[i].GetPriority();
        //         }
        //     }
        //     
        //     SetSelectableCard(selectable[index]);
        // }
        
        private CardHolder InstantSelect()
        {
            List<CardHolder> selectable = new List<CardHolder>();

            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0;
            
            foreach (CardHolder card in m_CardsOnBoards)
            {
                if (card.Selection.bounds.Contains(mousePosition))
                {
                    selectable.Add(card);
                }
            }

            if (selectable.Count == 0) return null;
            
            int biggestPriority = 0;
            int index = 0;

            for (int i = 0; i < selectable.Count; i++)
            {
                if (selectable[i].GetPriority() > biggestPriority)
                {
                    index = i;
                    biggestPriority = selectable[i].GetPriority();
                }
            }

            return selectable[index];
        }

        public void SetSelectableCard(CardHolder card)
        {
            m_SelectedCard = card;
        }
        

        public void GotoCard(CardState state, CardHolder card)
        {
            Holder currentHolder = card.GetComponentInParent<Holder>();

            card.UpdateState(state);
            currentHolder.RemoveCard(card);
            GetHolder(state).AddCard(card);

            m_SelectedCard = null;
            m_KeyPressed = KeyCode.None;
        }
        
        private void FecthAction()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                CardHolder cardSelected = InstantSelect();
                if(cardSelected)
                    cardSelected.RotateCard();
            }else if (Input.GetMouseButtonDown(0))
            {
                CardHolder cardSelected = InstantSelect();
                if(cardSelected && !CardHelpDisplay.Instance.InDisplay && !CardUIDisplay.Instance.InDisplay)
                    CardHelpDisplay.Instance.DisplayPreviewCard(cardSelected);
            }
            
            //Go to action
            if(Input.GetKeyDown(KeyCode.H))
            {
                SelectCard();
                m_KeyPressed = KeyCode.H;
            }else if(Input.GetKeyDown(KeyCode.D))
            {
                SelectCard();
                m_KeyPressed = KeyCode.D;
            }else if(Input.GetKeyDown(KeyCode.L))
            {
                SelectCard();
                m_KeyPressed = KeyCode.L;
            }else if(Input.GetKeyDown(KeyCode.G))
            {
                SelectCard();
                m_KeyPressed = KeyCode.G;
            }else if(Input.GetKeyDown(KeyCode.C))
            {
                SelectCard();
                m_KeyPressed = KeyCode.C;
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                SelectCard();
                m_KeyPressed = KeyCode.E;
            }else if(Input.GetKeyDown(KeyCode.A))
            {
                SelectCard();
                m_KeyPressed = KeyCode.A;
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
                case KeyCode.C:
                    return CardState.Creature;
                case KeyCode.E:
                    return CardState.Exil;
                case KeyCode.A:
                    return CardState.Enchantement;
                default:
                    return CardState.Hand;
            }
        }

        public Holder GetHolder(CardState state)
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
                case CardState.Creature:
                    return m_CreatureHolder;
                case CardState.Exil:
                    return m_ExileHolder;
                case CardState.Enchantement:
                    return m_EnchantementHolder;
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