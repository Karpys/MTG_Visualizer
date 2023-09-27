using System;
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
        
        [Header("Texture Manipulation Settings")]
        [SerializeField] private Vector2Int m_BorderSize = Vector2Int.zero;
        [SerializeField] private Color m_BorderColor = Color.black;
        
        private MagicApiRequest m_ApiRequest = null;

        private List<JObject> m_Cards = new List<JObject>();
        private void Awake()
        {
            m_ApiRequest = new MagicApiRequest();

            m_ApiRequest.OnCardFound += OnCardFound;
            m_ApiRequest.OnFailCardFound += OnFailCardfound;
            m_ApiRequest.OnCardPreview += OnCardDownload;
            m_ApiRequest.OnCardsFound += OnCardsFound;
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

        private void OnFailCardfound()
        {
            m_UIController.OnFailCardFound();
        }
        
        private void OnCardDownload(PreviewCardData cardDatas)
        {
            Sprite sprite = cardDatas.sprite;
            cardDatas.sprite = sprite;
            m_UIController.AddCard(sprite,cardDatas);
        }

        private void SetBorderColor(Texture2D texture)
        {
            int width = texture.width;
            int height = texture.height;
            
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    if (x <= m_BorderSize.x || x >= width - m_BorderSize.x || y <= m_BorderSize.y || y >= height - m_BorderSize.y)
                    {
                        texture.SetPixel(x,y,m_BorderColor);
                    }
                }
            }
            
            texture.Apply();
        }
        
        public void TryFindCard()
        {
            m_UIController.Clear();
            CancelMultipleDownload();
            m_ApiRequest.FindCard(m_UIController.GetCardName);
        }
        
        public void TryFindArtsCards()
        {
            m_UIController.Clear();
            CancelMultipleDownload();
            m_ApiRequest.FindCardsArts(m_UIController.GetCardName);
        }

        public void TryFindAbstractCards()
        {
            m_UIController.Clear();
            CancelMultipleDownload();
            m_ApiRequest.FindAbstractCards(m_UIController.GetCardName);
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
            m_ApiRequest.PreviewCard(cardObject,cancellationTokenSource);
        }
    }
}