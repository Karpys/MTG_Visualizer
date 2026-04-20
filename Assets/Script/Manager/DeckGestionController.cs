namespace Script.Manager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UI;
    using TMPro;
    using UnityEngine;
    using System.Collections;

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

        public DeckGestionContext DeckGestionContext => m_DeckGestionContext;

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
            DisplayCards();
            UpdateMaxPageCount();
            UpdatePageUI();
            ClearInDeckCards();
            ClearInDeckToken();
            ClearInDeckCommander();
            DisplayCurrentCommanderCards();
            DisplayCurrentDeckCards();
            DisplayCurrentTokenCards();
            UpdateCardCountText();
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
            
            for (int i = startIndex; i < endIndex; i++,y++)
            {
                m_CardDisplayer[y].gameObject.SetActive(true);

                m_CardsSprite.TryGetValue(m_CurrentCardsToDisplay[i].CardId, out CardDisplayData cardDisplayData);
                m_CardDisplayer[y].Initialize(m_CurrentCardsToDisplay[i].CardId,cardDisplayData);
            }

            for (; y < m_CardDisplayer.Length; y++)
            {
                m_CardDisplayer[y].gameObject.SetActive(false);
            }
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

        private void AddCardDisplayer(string id, int count)
        {
            if (m_CardsSprite.TryGetValue(id, out CardDisplayData cardDisplayData))
            {
                CardInDeckHolder cardInDeck = Instantiate(m_CardInDeckUIHolder, m_InDeckLayout);
                cardInDeck.Initialize(cardDisplayData,count,id,this);
                m_CurrentCardInDeck.Add(id,cardInDeck);
            }
        }

        private void AddTokenDisplayer(string id)
        {
            if (m_CardsSprite.TryGetValue(id, out CardDisplayData cardDisplayData))
            {
                SingleInDeckHolder singleInDeck = Instantiate(m_SingleInDeckUIHolder, m_TokenInDeckLayout);
                singleInDeck.Initialize(cardDisplayData,id,this,DeckGestionContext.Token);
                m_CurrentTokenInDeck.Add(id,singleInDeck);
            }
        }

        private void AddCommanderDisplayer(string id)
        {
            if (m_CardsSprite.TryGetValue(id, out CardDisplayData cardDisplayData))
            {
                SingleInDeckHolder singleInDeck = Instantiate(m_CommanderSingleUIHolder, m_InDeckLayout);
                singleInDeck.Initialize(cardDisplayData,id,this,DeckGestionContext.Commander);
                m_CurrentCommanderInDeck.Add(id,singleInDeck);
            }
        }
        
        public void AddCardInDeck(string currentCardId)
        {
            switch (DeckGestionContext)
            {
                case DeckGestionContext.Deck:
                    ChangeCardCount(currentCardId, 1, true);
                    break;
                case DeckGestionContext.Token:
                    AddToken(currentCardId);
                    break;
                case DeckGestionContext.Commander:
                    AddCommander(currentCardId);
                    break;
            }
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

        private void RemoveCardDisplayer(string id)
        {
            if (m_CurrentCardInDeck.TryGetValue(id, out CardInDeckHolder card))
            {
                Destroy(card.gameObject);
                m_CurrentCardInDeck.Remove(id);
            }
            
            UpdateCardCountText();
        }

        private void AddToken(string tokenId)
        {
            if(m_CurrentTokenInDeck.ContainsKey(tokenId))
                return;
            
            m_CurrentDeckData.TokenCards.Add(new CardCount(1,tokenId));
            AddTokenDisplayer(tokenId);
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
            }
            
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