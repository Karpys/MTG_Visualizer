using System.Collections.Generic;
using Manager;
using UnityEngine;

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
        [SerializeField] private JetonHolder m_JetonHolder = null;

        [Header("Parameters")] 
        [SerializeField] private bool m_DisplayHandCards = false;
        
        public List<CardHolder> m_CardsOnBoards = new List<CardHolder>();

        private List<CardScriptable> m_CardsInDeck = new List<CardScriptable>();
        private List<CardScriptable> m_JetonsCards = new List<CardScriptable>();
        public DeckScriptable Deck => m_Deck;
        private CardHolder m_SelectedCard = null;
        private KeyCode m_KeyPressed = KeyCode.None;

        public bool DisplayHandCards => m_DisplayHandCards;
        private void Start()
        {

            for (int i = 0; i < m_Deck.m_Cards.Count; i++)
            {
                m_CardsInDeck.Add(m_Deck.m_Cards[i]);
            }
            
            for (int i = 0; i < m_Deck.m_Terrains.Count; i++)
            {
                m_CardsInDeck.Add(m_Deck.m_Terrains[i]);
            }

            for (int i = 0; i < m_Deck.m_Jetons.Count; i++)
            {
                m_JetonsCards.Add(m_Deck.m_Jetons[i]);
            }
            
            m_CardsInDeck.Shuffle();


            for (int i = 0; i < m_CardsInDeck.Count; i++)
            {
                CardHolder card = Instantiate(Library.Instance.m_CardHolder, transform.position, Quaternion.identity,m_DeckHolder.transform);
                card.Initialize(m_CardsInDeck[i]);
                GotoCard(CardState.Deck,card);
                m_CardsOnBoards.Add(card);
            }
            
            for (int i = 0; i < m_Deck.m_Commanders.Count; i++)
            {
                CardHolder card = Instantiate(Library.Instance.m_CardHolder, transform.position, Quaternion.identity,m_EnchantementHolder.transform);
                card.Initialize(m_Deck.m_Commanders[i]);
                GotoCard(CardState.Enchantement,card);
                m_CardsOnBoards.Add(card);
            }
            
            for (int i = 0; i < m_JetonsCards.Count; i++)
            {
                CardHolder card = Instantiate(Library.Instance.m_CardHolder, transform.position, Quaternion.identity,m_DeckHolder.transform);
                card.Initialize(m_JetonsCards[i]);
                GotoCard(CardState.Jeton,card);
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

        private CardHolder TempSelect()
        {
            return InstantSelect();
        }
        
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
            card = TryGetJeton(card);
            Holder currentHolder = card.GetComponentInParent<Holder>();

            card.UpdateState(state);
            currentHolder.RemoveCard(card);
            GetHolder(state).AddCard(card);

            m_SelectedCard = null;
            m_KeyPressed = KeyCode.None;
        }

        private CardHolder TryGetJeton(CardHolder card)
        {
            if (card.State != CardState.Jeton) return card;
            
            CardHolder newCard = Instantiate(Library.Instance.m_CardHolder, transform.position, Quaternion.identity,m_JetonHolder.transform);
            newCard.Initialize(card);
            m_CardsOnBoards.Add(newCard);
            return newCard;
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
            }else if (Input.GetKeyDown(KeyCode.J))
            {
                CardUIDisplay.Instance.DisplayCard(GetHolder(GetState(KeyCode.J)).Cards, GetState(KeyCode.J));
            }else if (Input.GetAxis("Mouse ScrollWheel") > 0f )
            {
                CardHolder card = TempSelect();
                card.ChangeCounter(1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                CardHolder card = TempSelect();
                card.ChangeCounter(-1);
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
                case KeyCode.J:
                    return CardState.Jeton;
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
                case CardState.Jeton:
                    return m_JetonHolder;
                default:
                    return m_HandHolder;
            }
        }

        public CardHolder GetCard(CardState state, int cardId)
        {
            Holder holder = GetHolder(state);
            return holder.Cards[cardId];
        }

        public void Draw()
        {
            GotoCard(CardState.Hand,m_DeckHolder.Cards[0]);
        }
    }
}