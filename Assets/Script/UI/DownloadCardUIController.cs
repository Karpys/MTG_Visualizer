using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class DownloadCardUIController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_CardField = null;
        [SerializeField] private TMP_Text m_CardFoundText = null;
        [SerializeField] private Transform m_DownloadButton = null;

        [Header("Card Layout")] 
        [SerializeField] private Transform m_CardLayout = null;
        [SerializeField] private GameObject m_CardLayoutPrefab = null;

        public string GetCardName => m_CardField.text;
        public void DisplayDownloadContainer(int cardCount = 0)
        {
            string additionalText = "";

            if (cardCount > 0)
            {
                additionalText = cardCount+" ";
            }
            
            m_CardFoundText.gameObject.SetActive(true);
            m_CardFoundText.text = additionalText + "Card Found";
            m_DownloadButton.gameObject.SetActive(true);
        }


        public void OnFailCardFound()
        {
            m_CardFoundText.text = "No Card Found";
            m_DownloadButton.gameObject.SetActive(false);
            m_CardLayout.gameObject.SetActive(false);
        }

        public void AddCard(Sprite sprite, CardData cardDatas)
        {
            m_CardLayout.gameObject.SetActive(true);
            GameObject card = Instantiate(m_CardLayoutPrefab, m_CardLayout);
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
        }
    }
}