using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class MagicApiRequestHolder : MonoBehaviour
    {
        [SerializeField] private DownloadCardUIController m_UIController = null;
        [SerializeField] private SetLangHolder[] m_SetLangHolders = null;
        [SerializeField] private CardTypeFilter m_CardTypeFilter = null;
        [SerializeField] private MinMaxCostFilter m_MinMaxCostFilter = null;
        [SerializeField] private MinMaxCostFilter m_MinMaxPowFilter = null;
        [SerializeField] private MinMaxCostFilter m_MinMaxTouFilter = null;
        [SerializeField] private ManaCostFilter m_ManaCostFilter = null;
        
        private MagicApiRequest m_ApiRequest = null;

        private List<JObject> m_Cards = new List<JObject>();
        private void Awake()
        {
            m_ApiRequest = new MagicApiRequest();

            m_ApiRequest.OnCardFound += OnCardFound;
            m_ApiRequest.OnFailCardFound += OnFailCardFound;
            m_ApiRequest.OnCardPreview += OnCardDownload;
            m_ApiRequest.OnCardsFound += OnCardsFound;
        }

        public void SetLang(string lang)
        {
            foreach (SetLangHolder setLangHolder in m_SetLangHolders)
            {
                setLangHolder.Enable(setLangHolder.Lang == lang);
            }
            m_ApiRequest.SetLang(lang);
        }

        private void OnCardFound(JObject card)
        {
            m_Cards.Clear();
            m_Cards.Add(card);
            m_UIController.DisplayDisplayContainer(m_Cards.Count);
        }
        
        private void OnCardsFound(JObject[] cards)
        {
            m_Cards = cards.ToList();
            m_UIController.DisplayDisplayContainer(m_Cards.Count);
        }

        private void OnFailCardFound()
        {
            m_UIController.OnFailCardFound();
        }
        
        private void OnCardDownload(ApiCardData cardDatas,JObject cardObject)
        {
            Sprite sprite = cardDatas.m_FrontCardSprite;
            cardDatas.m_FrontCardSprite = sprite;
            m_UIController.AddCard(sprite,cardDatas,cardObject);
        }

        public void TryFindCard()
        {
            m_UIController.Clear();
            CancelMultipleDownload();
            _ = m_ApiRequest.FindCard(m_UIController.GetCardName);
        }
        
        public void TryFindArtsCards()
        {
            m_UIController.Clear();
            CancelMultipleDownload();
            _ = m_ApiRequest.FindCardsArts(m_UIController.GetCardName);
        }

        public void TryFindAbstractCards()
        {
            m_UIController.Clear();
            CancelMultipleDownload();
            _ = m_ApiRequest.FindAbstractCards(GetFilters());
        }

        private Task m_CurrentPreviewCardsTask = null;
        private CancellationTokenSource m_CancellationTokenSource = null;

        public void PreviewCards()
        {
            m_UIController.Clear();
            CancelMultipleDownload();
            
            m_CancellationTokenSource = new CancellationTokenSource();
            if (m_Cards.Count > 1)
            {
                m_CurrentPreviewCardsTask = m_ApiRequest.PreviewCards(m_Cards,m_CancellationTokenSource);
            }
            else
            {
                PreviewCardImage(m_Cards[0],m_CancellationTokenSource);
            }
        }

        private void CancelMultipleDownload()
        {
            if (m_CurrentPreviewCardsTask is {IsCompleted: false})
            {
                m_CancellationTokenSource.Cancel();
            }
        }

        private void PreviewCardImage(JObject cardObject,CancellationTokenSource cancellationTokenSource)
        {
            _ = m_ApiRequest.PreviewCard(cardObject,cancellationTokenSource);
        }

        private string GetNameFilter()
        {
            string cardName = m_UIController.GetCardName;

            if (cardName.Length == 0)
                return "";
            return "+name:" + cardName;
        }
        
        private string GetFilters()
        {
            return GetNameFilter() + m_CardTypeFilter.GetFilter() + m_MinMaxCostFilter.GetFilter() + m_MinMaxPowFilter.GetFilter()
                   + m_MinMaxTouFilter.GetFilter() + m_ManaCostFilter.GetFilter();
        }
    }
}