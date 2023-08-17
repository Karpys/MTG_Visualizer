using System;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Random = UnityEngine.Random;

namespace Script
{
    public class MagicApiRequestHolder : MonoBehaviour
    {
        [SerializeField] private string m_CardName = String.Empty;
        [SerializeField] private SpriteRenderer m_TargetRenderer = null;
        
        [Header("Texture Manipulation Settings")]
        [SerializeField] private Vector2Int m_BorderSize = Vector2Int.zero;
        [SerializeField] private Color m_BorderColor = Color.black;
        
        private MagicApiRequest m_ApiRequest = null;

        private JObject m_currentCard;
        private void Awake()
        {
            m_ApiRequest = new MagicApiRequest();

            m_ApiRequest.OnCardFound += OnCardFound;
            m_ApiRequest.OnCardDownload += OnCardDownload;
        }

        private void OnCardFound(JObject card)
        {
            m_currentCard = card;
        }
        
        private void OnCardDownload(string cardPath)
        {
            byte[] cardData = File.ReadAllBytes(cardPath);
            Texture2D cardTexture = new Texture2D(488, 680);
            cardTexture.LoadImage(cardData);
            ApplyModification(cardTexture);
            Sprite sprite = Sprite.Create(cardTexture,new Rect(Vector2.zero,new Vector2(488,680)),Vector2.zero);
            m_TargetRenderer.sprite = sprite;
        }

        private void ApplyModification(Texture2D texture)
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

        private void Update()
        {
            FindCardInput();
            DownloadCardInput();
        }

        private void FindCardInput()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                TryFindCard();
            }
        }

        private void TryFindCard()
        {
            m_ApiRequest.FindCard(m_CardName);
        }

        private void DownloadCardInput()
        {
            if (Input.GetKeyDown(KeyCode.D) && m_currentCard != null)
            {
                DownloadImage(m_currentCard);            
            }
        }

        private void DownloadImage(JObject cardObject)
        {
            m_ApiRequest.DownloadCard(cardObject);
        }
    }
}