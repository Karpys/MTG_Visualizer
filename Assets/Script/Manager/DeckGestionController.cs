namespace Script.Manager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UI;
    using TMPro;
    using UnityEngine;
    using System.Collections;
    using Helper;
    using KarpysDev.KarpysUtils;

    public struct CardDisplayData
    {
        public Sprite m_FrontSprite;
        public Sprite m_BackSprite;
    }

    public enum DeckGestionContext
    {
        Deck,
        Token,
        Commander,
    }

    public class DeckGestionController : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_CardCountInDeckText = null;
        [SerializeField] private CardGridSetter m_CardGridSetter = null;
        [SerializeField] private CardInLibraryPointer[] m_CardDisplayer = null;
        [SerializeField] private TMP_Text m_CardPageCount = null;
        [SerializeField] private TMP_InputField m_FilterInput = null;
        [SerializeField] private DeckGestionContextDisplayer m_DeckGestionContextDisplayer;
        [SerializeField] private DeckGestionCardViewerController m_DeckCardViewerController = null;
        [SerializeField] private UIDragController m_DragController = null;
        [SerializeField] private RectTransform m_DeckRect = null;
        [SerializeField] private float m_MinDistanceInsert = 1f;

        [Header("Card in Deck")] 
        [SerializeField] private Transform m_InDeckLayout = null;
        [SerializeField] private Transform m_TokenInDeckLayout = null;
        [SerializeField] private CardInDeckHolder m_CardInDeckUIHolder = null;
        [SerializeField] private SingleInDeckHolder m_SingleInDeckUIHolder = null;
        [SerializeField] private SingleInDeckHolder m_CommanderSingleUIHolder = null;

        private Dictionary<string, CardDisplayData> m_CardsSprite = new Dictionary<string, CardDisplayData>();
        private List<LibraryCardData> m_CurrentCardsToDisplay = null;
        private DeckGestionContext m_DeckGestionContext = DeckGestionContext.Deck;
        
        private List<LibraryCardData> m_CardsInLibrary = null;

        private int CARD_COUNT_DISPLAY = 20;
        private int m_CurrentPage = 0;
        private int m_CurrentMaxPage = 0;
        private DeckData m_CurrentDeckData;

        private Dictionary<string,CardInDeckHolder> m_CurrentCardInDeck = new Dictionary<string,CardInDeckHolder>();
        private Dictionary<string,SingleInDeckHolder> m_CurrentTokenInDeck = new Dictionary<string,SingleInDeckHolder>();
        private Dictionary<string,SingleInDeckHolder> m_CurrentCommanderInDeck = new Dictionary<string,SingleInDeckHolder>();
        
        private List<IUIDraggable> m_CardDeckHolderDraggables = new List<IUIDraggable>();
        private List<IUIDraggable> m_CardInLibraryDraggables = new List<IUIDraggable>();
        

        public DeckGestionContext DeckGestionContext => m_DeckGestionContext;
        public RectTransform DeckRect => m_DeckRect;

        private void Awake()
        {
            m_FilterInput.onValueChanged.AddListener(ApplyNameFilter);
        }

        private void Update()
        {
            CARD_COUNT_DISPLAY = m_CardGridSetter.CardCount;
        }

        private void OnEnable()
        {
            m_CurrentPage = 0;
            FetchCardsInLibrary();
            m_CurrentCardsToDisplay = new List<LibraryCardData>(m_CardsInLibrary);
            StartCoroutine(Enable());
        }

        private IEnumerator Enable()
        {
            yield return new WaitForSeconds(0.1f);
            GenerateCardsSprite();
            ClearInDeckCards();
            ClearInDeckToken();
            ClearInDeckCommander();
            DisplayCurrentCommanderCards();
            DisplayCurrentDeckCards();
            DisplayCurrentTokenCards();
            DisplayCards();
            UpdateMaxPageCount();
            UpdatePageUI();
            UpdateCardCountText();
            UpdateDraggables();
        }

        public void SetDeckContext()
        {
            m_DeckGestionContext = DeckGestionContext.Deck;
            m_DeckGestionContextDisplayer.Display(m_DeckGestionContext);
        }
        
        public void SetTokenContext()
        {
            m_DeckGestionContext = DeckGestionContext.Token;
            m_DeckGestionContextDisplayer.Display(m_DeckGestionContext);
        }

        public void SetCommanderContext()
        {
            m_DeckGestionContext = DeckGestionContext.Commander;
            m_DeckGestionContextDisplayer.Display(m_DeckGestionContext);
        }

        private void UpdateMaxPageCount()
        {
            m_CurrentMaxPage = Math.Max(0,(int)Mathf.Floor(((float)m_CurrentCardsToDisplay.Count - 1)  / CARD_COUNT_DISPLAY));
            UpdatePageUI();
        }

        private void UpdatePageUI()
        {
            m_CardPageCount.text = (m_CurrentPage+1) + "/" + (m_CurrentMaxPage+1);
        }

        public void NextPage()
        {
            if(m_CurrentPage == m_CurrentMaxPage)
                return;

            m_CurrentPage++;
            DisplayCards();
            UpdatePageUI();
        }

        public void PreviousPage()
        {
            if(m_CurrentPage == 0)
                return;

            m_CurrentPage--;
            DisplayCards();
            UpdatePageUI();
        }

        private void DisplayCards()
        {
            int startIndex = m_CurrentPage * CARD_COUNT_DISPLAY;
            //Replace by CardInCurrentResearchFilter//
            int endIndex = Math.Min(m_CurrentCardsToDisplay.Count, startIndex + CARD_COUNT_DISPLAY);

            int y = 0;

            ApplyAlphabeticalFilter();

            List<IUIDraggable> draggables = new List<IUIDraggable>(m_CardDeckHolderDraggables);
            
            for (int i = startIndex; i < endIndex; i++,y++)
            {
                m_CardDisplayer[y].gameObject.SetActive(true);

                m_CardsSprite.TryGetValue(m_CurrentCardsToDisplay[i].CardId, out CardDisplayData cardDisplayData);
                m_CardDisplayer[y].Initialize(m_CurrentCardsToDisplay[i].CardId,cardDisplayData);
                draggables.Add(m_CardDisplayer[y].Draggable);
            }

            m_CardInLibraryDraggables = draggables;
            
            for (; y < m_CardDisplayer.Length; y++)
            {
                m_CardDisplayer[y].gameObject.SetActive(false);
            }

            UpdateDraggables();
        }

        private void UpdateDraggables()
        {
            List<IUIDraggable> draggables = new List<IUIDraggable>();

            foreach (IUIDraggable draggable in m_CardDeckHolderDraggables)
            {
                draggables.Add(draggable);
            }
            
            foreach (IUIDraggable draggable in m_CardInLibraryDraggables)
            {
                draggables.Add(draggable);
            }
            
            m_DragController.SetDraggable(draggables);
        }

        private void ApplyAlphabeticalFilter()
        {
            m_CurrentCardsToDisplay.Sort((a,b) => string.Compare(a.CardName,b.CardName,StringComparison.Ordinal));
        }

        private void ApplyNameFilter(string filter)
        {
            m_CurrentCardsToDisplay.Clear();

            for (int i = 0; i < m_CardsInLibrary.Count; i++)
            {
                if (m_CardsInLibrary[i].CardName.Contains(filter,StringComparison.OrdinalIgnoreCase))
                {
                    m_CurrentCardsToDisplay.Add(m_CardsInLibrary[i]);
                }
            }

            m_CurrentPage = 0;
            UpdateMaxPageCount();
            DisplayCards();
        }
        
        private void FetchCardsInLibrary()
        {
            CardFileHelper.UpdateLibrary();
            m_CardsInLibrary = CardFileHelper.GetCardsInLibrary();
        }

        private void GenerateCardsSprite()
        {
            for (int i = 0; i < m_CardsInLibrary.Count; i++)
            {
                if(m_CardsSprite.ContainsKey(m_CardsInLibrary[i].CardId))
                    continue;

                Sprite[] cardSprites = m_CardsInLibrary[i].ToCardSprite();
                CardDisplayData cardDisplayData = new CardDisplayData();
                cardDisplayData.m_FrontSprite = cardSprites[0];
                cardDisplayData.m_BackSprite = cardSprites[1];
                m_CardsSprite.Add(m_CardsInLibrary[i].CardId,cardDisplayData);
            }
        }

        public void SetDeckData(DeckData deckData)
        {
            m_CurrentDeckData = deckData;
        }

        private void ClearInDeckCards()
        {
            foreach (CardInDeckHolder cardInDeckHolder in m_CurrentCardInDeck.Values)
            {
                Destroy(cardInDeckHolder.gameObject);
            }
            
            m_CardDeckHolderDraggables.Clear();
            m_CurrentCardInDeck.Clear();
        }
        
        private void ClearInDeckToken()
        {
            foreach (SingleInDeckHolder token in m_CurrentTokenInDeck.Values)
            {
                Destroy(token.gameObject);
            }
            
            m_CurrentTokenInDeck.Clear();
        }
        
        private void ClearInDeckCommander()
        {
            foreach (SingleInDeckHolder commander in m_CurrentCommanderInDeck.Values)
            {
                Destroy(commander.gameObject);
            }
            
            m_CurrentCommanderInDeck.Clear();
        }
        
        
        private void DisplayCurrentDeckCards()
        {
            for (int i = 0; i < m_CurrentDeckData.DeckCards.Count; i++)
            {
                AddCardDisplayer(m_CurrentDeckData.DeckCards[i].CardId,m_CurrentDeckData.DeckCards[i].Count);
            }
        }

        private void DisplayCurrentTokenCards()
        {
            for (int i = 0; i < m_CurrentDeckData.TokenCards.Count; i++)
            {
                AddTokenDisplayer(m_CurrentDeckData.TokenCards[i].CardId);
            }
        }

        private void DisplayCurrentCommanderCards()
        {
            for (int i = 0; i < m_CurrentDeckData.CommanderCards.Count; i++)
            {
                AddCommanderDisplayer(m_CurrentDeckData.CommanderCards[i].CardId);
            }
        }

        public void AssignAsCommander(string id)
        {
            if(m_CurrentCardInDeck.TryGetValue(id, out CardInDeckHolder holder))
            {
                Destroy(holder.gameObject);
                m_CurrentCardInDeck.Remove(id);

                for (int i = 0; i < m_CurrentDeckData.DeckCards.Count; i++)
                {
                    if (m_CurrentDeckData.DeckCards[i].CardId == id)
                        m_CurrentDeckData.DeckCards.RemoveAt(i);
                }
                
                AddCommander(id);
            }
            
            UpdateCardCountText();
        }

        private CardInDeckHolder AddCardDisplayer(string id, int count)
        {
            if (m_CardsSprite.TryGetValue(id, out CardDisplayData cardDisplayData))
            {
                CardInDeckHolder cardInDeck = Instantiate(m_CardInDeckUIHolder, m_InDeckLayout);
                cardInDeck.InitializeBaseCard(cardDisplayData,id,this);
                cardInDeck.Initialize(count);
                cardInDeck.InitializeDragg();
                cardInDeck.name = id;
                m_CurrentCardInDeck.Add(id,cardInDeck);
                m_CardDeckHolderDraggables.Add(cardInDeck.Draggable);
                UpdateDraggables();
                return cardInDeck;
            }

            return null;
        }

        private void AddTokenDisplayer(string id)
        {
            if (m_CardsSprite.TryGetValue(id, out CardDisplayData cardDisplayData))
            {
                SingleInDeckHolder singleInDeck = Instantiate(m_SingleInDeckUIHolder, m_TokenInDeckLayout);
                singleInDeck.InitializeBaseCard(cardDisplayData,id,this);
                singleInDeck.Initialize(DeckGestionContext.Token);
                m_CurrentTokenInDeck.Add(id,singleInDeck);
            }
        }

        private void AddCommanderDisplayer(string id)
        {
            if (m_CardsSprite.TryGetValue(id, out CardDisplayData cardDisplayData))
            {
                SingleInDeckHolder singleInDeck = Instantiate(m_CommanderSingleUIHolder, m_InDeckLayout);
                singleInDeck.InitializeBaseCard(cardDisplayData,id,this);
                singleInDeck.Initialize(DeckGestionContext.Commander);
                m_CurrentCommanderInDeck.Add(id,singleInDeck);
            }
        }
        
        private void AddCardInDeck(string currentCardId)
        {
            switch (DeckGestionContext)
            {
                case DeckGestionContext.Deck:
                    AddDeckCard(currentCardId);
                    break;
                case DeckGestionContext.Token:
                    AddToken(currentCardId);
                    break;
                case DeckGestionContext.Commander:
                    AddCommander(currentCardId);
                    break;
            }
        }

        private void InsertDeckCard(string id, int positionInsert, DeckGestionContext context)
        {
            switch (context)
            {
                case DeckGestionContext.Deck:
                    if (m_CurrentDeckData.DeckCards.Count == 0)
                    {
                        m_CurrentDeckData.DeckCards.Add(new CardCount(1, id));
                        return;
                    }
                    
                    m_CurrentDeckData.DeckCards.Insert(positionInsert - 1, new CardCount(1,id));
                    break;
                case DeckGestionContext.Token:
                    
                    if (m_CurrentDeckData.TokenCards.Count == 0)
                    {
                        m_CurrentDeckData.TokenCards.Add(new CardCount(1, id));
                        return;
                    }

                    m_CurrentDeckData.TokenCards.Insert(positionInsert - 1, new CardCount(1,id));
                    break;
                case DeckGestionContext.Commander:
                    
                    if (m_CurrentDeckData.CommanderCards.Count == 0)
                    {
                        m_CurrentDeckData.CommanderCards.Add(new CardCount(1, id));
                        return;
                    }

                    m_CurrentDeckData.CommanderCards.Insert(positionInsert - 1, new CardCount(1,id));
                    break;
            }
        }

        private void InsertCardViaPosition(Vector3 position, BaseCardInDeckUI card, out int idPosition)
        {
            List<Transform> cardInDeck = new List<Transform>();
            int lowestSiblingIndex = 1000;
            
            foreach (KeyValuePair<string, CardInDeckHolder> cardInDeckHolder in m_CurrentCardInDeck)
            {
                cardInDeck.Add(cardInDeckHolder.Value.transform);
                int siblingIndex = cardInDeckHolder.Value.transform.GetSiblingIndex();

                if (siblingIndex < lowestSiblingIndex)
                    lowestSiblingIndex = siblingIndex;
            }

            Transform closest = cardInDeck.GetClosest(position);

            if (Vector3.Distance(closest.position, position) >= m_MinDistanceInsert)
            {
                idPosition = -1;
            }
            else
            {
                card.transform.SetSiblingIndex(closest.GetSiblingIndex());
                idPosition = closest.GetSiblingIndex() - lowestSiblingIndex;
            }
        }

        private BaseCardInDeckUI GetCard(string cardId, DeckGestionContext context)
        {
            switch (context)
            {
                case DeckGestionContext.Deck:
                    
                    foreach (KeyValuePair<string, CardInDeckHolder> cardInDeckHolder in m_CurrentCardInDeck)
                    {
                        if (cardInDeckHolder.Value.Id == cardId)
                            return cardInDeckHolder.Value;
                    }
                    
                    break;
                case DeckGestionContext.Token:
                    
                    foreach (KeyValuePair<string, SingleInDeckHolder> commanderInDeck in m_CurrentCommanderInDeck)
                    {
                        if (commanderInDeck.Value.Id == cardId)
                            return commanderInDeck.Value;
                    }
                    
                    break;
                case DeckGestionContext.Commander:
                    
                    foreach (KeyValuePair<string, SingleInDeckHolder> tokenInDeck in m_CurrentTokenInDeck)
                    {
                        if (tokenInDeck.Value.Id == cardId)
                            return tokenInDeck.Value;
                    }
                    
                    break;
            }

            return null;
        }
        
        public void InsertCardInDeckAt(Vector3 mousePosition, string cardId)
        {
            switch (DeckGestionContext)
            {
                case DeckGestionContext.Deck:

                    bool cardExist = CardExist(cardId, DeckGestionContext.Deck);
                    
                    if (!cardExist)
                    {
                        CardInDeckHolder card = AddCardDisplayer(cardId,1);
                        InsertCardViaPosition(mousePosition, card, out int idPosition);

                        if (idPosition == -1)
                        {
                            m_CurrentDeckData.DeckCards.Add(new CardCount(1,cardId));
                        }
                        else
                        {
                            InsertDeckCard(cardId, idPosition, DeckGestionContext.Deck);
                        }
                        
                        UpdateCardCountText();
                    }
                    else
                    {
                        ChangeCardCount(cardId, 1, true);
                    }
                    
                    break;
                case DeckGestionContext.Token:
                    AddToken(cardId);
                    break;
                case DeckGestionContext.Commander:
                    AddCommander(cardId);
                    break;
            }
        }

        private bool CardExist(string cardId, DeckGestionContext deck)
        {
            switch (deck)
            {
                case DeckGestionContext.Deck:

                    foreach (CardCount cardCount in m_CurrentDeckData.DeckCards)
                    {
                        if (cardCount.CardId == cardId)
                            return true;
                    }
                    
                    break;
                case DeckGestionContext.Token:
                    
                    foreach (CardCount cardCount in m_CurrentDeckData.TokenCards)
                    {
                        if (cardCount.CardId == cardId)
                            return true;
                    }
                    
                    break;
                case DeckGestionContext.Commander:
                    
                    foreach (CardCount cardCount in m_CurrentDeckData.CommanderCards)
                    {
                        if (cardCount.CardId == cardId)
                            return true;
                    }
                    
                    break;
            }

            return false;
        }

        public int ChangeCardCount(string cardId, int currentCount, bool updateDisplayer = false)
        {
            for (int i = 0; i < m_CurrentDeckData.DeckCards.Count; i++)
            {
                CardCount cardCount = m_CurrentDeckData.DeckCards[i];
                if (cardCount.CardId == cardId)
                {
                    cardCount.Count += currentCount;
                    
                    if (cardCount.Count == 0)
                    {
                        m_CurrentDeckData.DeckCards.RemoveAt(i);
                        RemoveCardDisplayer(cardId);
                        return cardCount.Count;
                    }

                    if (updateDisplayer)
                    {
                        m_CurrentCardInDeck.TryGetValue(cardId, out CardInDeckHolder cardInDeck);
                        if (cardInDeck != null) cardInDeck.UpdateCoutUI(cardCount.Count);
                    }
                    
                    m_CurrentDeckData.DeckCards[i] = cardCount;
                    UpdateCardCountText();
                    return cardCount.Count;
                }
            }

            m_CurrentDeckData.DeckCards.Add(new CardCount(1,cardId));
            AddCardDisplayer(cardId,currentCount);
            UpdateCardCountText();
            return 0;
        }

        public int RemoveCardCount(string cardId,int count, out bool hasRemove)
        {
            hasRemove = false;
            
            for (int i = 0; i < m_CurrentDeckData.DeckCards.Count; i++)
            {
                CardCount cardCount = m_CurrentDeckData.DeckCards[i];
                if (cardCount.CardId == cardId)
                {
                    cardCount.Count -= count;
                    
                    if (cardCount.Count == 0)
                    {
                        m_CurrentDeckData.DeckCards.RemoveAt(i);
                        RemoveCardDisplayer(cardId);
                        hasRemove = true;
                        return cardCount.Count;
                    }

                    m_CurrentCardInDeck.TryGetValue(cardId, out CardInDeckHolder cardInDeck);
                    if (cardInDeck != null) cardInDeck.UpdateCoutUI(cardCount.Count);
                    
                    m_CurrentDeckData.DeckCards[i] = cardCount;
                    UpdateCardCountText();
                    return cardCount.Count;
                }
            }
            
            return 0;
        }

        private void RemoveCardDisplayer(string id)
        {
            if (m_CurrentCardInDeck.TryGetValue(id, out CardInDeckHolder card))
            {
                m_CardDeckHolderDraggables.Remove(card.Draggable);
                m_CurrentCardInDeck.Remove(id);
                Destroy(card.gameObject);
            }
            
            UpdateDraggables();
            UpdateCardCountText();
        }

        private void AddToken(string tokenId)
        {
            if(m_CurrentTokenInDeck.ContainsKey(tokenId))
                return;
            
            m_CurrentDeckData.TokenCards.Add(new CardCount(1,tokenId));
            AddTokenDisplayer(tokenId);
        }
        
        private void AddDeckCard(string id)
        {
            m_CurrentDeckData.DeckCards.Add(new CardCount(1,id));
            AddCardDisplayer(id,1);
        }
        
        private void AddCommander(string cardId)
        {
            if(m_CurrentCommanderInDeck.ContainsKey(cardId))
                return;
            
            m_CurrentDeckData.CommanderCards.Add(new CardCount(1,cardId));
            AddCommanderDisplayer(cardId);
        }

        
        public void RemoveCard(string cardId, DeckGestionContext context)
        {
            switch (context)
            {
                case DeckGestionContext.Token:
                    
                    if (m_CurrentTokenInDeck.TryGetValue(cardId, out SingleInDeckHolder token))
                    {
                        Destroy(token.gameObject);
                        m_CurrentTokenInDeck.Remove(cardId);
                        m_CurrentDeckData.TokenCards.Remove(new CardCount(1, cardId)); 
                    }
                    
                    break;
                case DeckGestionContext.Commander:
                    
                    if (m_CurrentCommanderInDeck.TryGetValue(cardId, out SingleInDeckHolder commander))
                    {
                        Destroy(commander.gameObject);
                        m_CurrentCommanderInDeck.Remove(cardId);
                        m_CurrentDeckData.CommanderCards.Remove(new CardCount(1, cardId)); 
                    }
                    break;
                
                case DeckGestionContext.Deck:
                    RemoveCardDisplayer(cardId);
                    break;
            }
        }

        private void TempRemoveCardFromDeck(string cardId)
        {
            if (m_CurrentCardInDeck.TryGetValue(cardId, out CardInDeckHolder card))
            {
                card.gameObject.SetActive(false);
                m_CurrentCardInDeck.Remove(cardId);
                m_CardDeckHolderDraggables.Remove(card.Draggable);
            }
            
            UpdateDraggables();
            UpdateCardCountText();
        }

        private void UpdateCardCountText()
        {
            int cardCount = 0;

            foreach (CardCount deckCard in m_CurrentDeckData.DeckCards)
            {
                cardCount += deckCard.Count;
            }
            
            m_CardCountInDeckText.text = cardCount.ToString();
        }

        public void SaveDeck()
        {
            string[] deckData = m_CurrentDeckData.ToFile();
            File.WriteAllLines(CardFileHelper.GetDeckPath() + m_CurrentDeckData.DeckName + ".deck",deckData);
        }

        public void DisplaySingleCard(CardDisplayData cardDisplayData)
        {
            m_DeckCardViewerController.DisplaySingle(cardDisplayData);
        }

        public void DisplayCardInDeck(CardDisplayData cardDisplayData, string cardId)
        {
            m_DeckCardViewerController.DisplayInDeck(this,cardDisplayData,cardId);
        }

        public int GetCardCount(string id)
        {
            foreach (CardCount cardCount in m_CurrentDeckData.DeckCards)
            {
                if (cardCount.CardId == id)
                    return cardCount.Count;
            }

            return 0;
        }
    }

    public struct LibraryCardData
    {
        public string CardPathName;
        public string CardImagePath;
        public string CardFileName;
        public string CardName;
        public string CardId;
        public bool IsDualCard;

        public LibraryCardData(string cardName, string cardId,string cardFileName,string cardPathName,string cardImagePath, bool isDualCard)
        {
            CardName = cardName;
            CardId = cardId;
            CardFileName = cardFileName;
            CardPathName = cardPathName;
            CardImagePath = cardImagePath;
            IsDualCard = isDualCard;
        }
    }
}