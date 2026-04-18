using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public class DownloadCardUIController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_CardField = null;
        [SerializeField] private TMP_Text m_CardFoundText = null;
        [SerializeField] private Transform m_DisplayButton = null;
        [SerializeField] private DownloadCardButton m_DownloadButtonTransform = null;
        [SerializeField] private ApiResearchDisplayCardViewer m_CardViewer = null;

        [Header("Card Layout")] 
        [SerializeField] private Transform m_CardLayout = null;
        [SerializeField] private DownLoadCardDataPointer m_CardDataPointer = null;

        private int m_CurrentPosition = 0;
        public string GetCardName => m_CardField.text;

        private List<DownLoadCardDataPointer> m_CardDisplay = new List<DownLoadCardDataPointer>();

        private void Update()
        {
            if(!m_CardViewer.InDisplay)
                return;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Next();
            }else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Previous();
            }
        }

        public void DisplayDisplayContainer(int cardCount = 0)
        {
            string additionalText = "";

            if (cardCount > 0)
            {
                additionalText = cardCount+" ";
            }
            
            m_CardFoundText.gameObject.SetActive(true);
            m_CardFoundText.text = additionalText + "Card Found";
            m_DisplayButton.gameObject.SetActive(true);
            m_DownloadButtonTransform.gameObject.SetActive(false);
        }


        public void OnFailCardFound()
        {
            m_CardFoundText.gameObject.SetActive(true);
            m_CardFoundText.text = "No Card Found";
            m_DisplayButton.gameObject.SetActive(false);
            m_CardLayout.gameObject.SetActive(false);
        }

        public void AddCard(ApiCardData cardDatas, JObject cardObject)
        {
            m_CardLayout.gameObject.SetActive(true);
            DownLoadCardDataPointer card = Instantiate(m_CardDataPointer, m_CardLayout);
            card.Initialize(cardObject,cardDatas,m_DownloadButtonTransform,this,m_CardDisplay.Count);
            m_CardDisplay.Add(card);
        }

        public void Clear()
        {
            int childCount = m_CardLayout.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = m_CardLayout.GetChild(i);
                Destroy(child.gameObject);
            }
            
            m_CardDisplay.Clear();
            m_DownloadButtonTransform.gameObject.SetActive(false);
            m_CurrentPosition = 0;
        }

        public void DisplayCard(int position)
        {
            if(m_CardDisplay.Count == 0)
                return;
            m_CurrentPosition = position;
            m_CardViewer.DisplayApiCard(m_CardDisplay[position].CardData);
        }

        private void Next()
        {
            int nextPosition = Math.Min(m_CardDisplay.Count - 1, m_CurrentPosition + 1);
            DisplayCard(nextPosition);
        }

        private void Previous()
        {
            int previousPosition = Math.Max(0, m_CurrentPosition - 1);
            DisplayCard(previousPosition);
        }
    }
}