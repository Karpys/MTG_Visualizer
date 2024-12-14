using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    using Newtonsoft.Json.Linq;

    public class DownloadCardUIController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_CardField = null;
        [SerializeField] private TMP_Text m_CardFoundText = null;
        [SerializeField] private Transform m_DisplayButton = null;
        [SerializeField] private DownloadCardButton m_DownloadButtonTransform = null;

        [Header("Card Layout")] 
        [SerializeField] private Transform m_CardLayout = null;
        [SerializeField] private DownLoadCardDataPointer m_CardDataPointer = null;

        public string GetCardName => m_CardField.text;

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
            m_CardFoundText.text = "No Card Found";
            m_DisplayButton.gameObject.SetActive(false);
            m_CardLayout.gameObject.SetActive(false);
        }

        public void AddCard(Sprite sprite, PreviewCardData cardDatas, JObject cardObject)
        {
            m_CardLayout.gameObject.SetActive(true);
            DownLoadCardDataPointer card = Instantiate(m_CardDataPointer, m_CardLayout);
            card.Initialize(cardObject,cardDatas,m_DownloadButtonTransform);
            card.GetComponent<Image>().sprite = sprite;
        }

        public void Clear()
        {
            int childCount = m_CardLayout.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = m_CardLayout.GetChild(i);
                Destroy(child.gameObject);
            }
            
            m_DownloadButtonTransform.gameObject.SetActive(false);
        }
    }
}