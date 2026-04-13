namespace Script.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class DisplayCardViewer : MonoBehaviour
    {
        [SerializeField] private Transform m_CardViewer = null;
        [SerializeField] private Image m_Image = null;
        [SerializeField] private Transform m_SwapButton = null;

        private bool m_DisplayFront = true;
        private Sprite m_FrontSprite = null;
        private Sprite m_BackSprite = null;
        
        public void Display(Sprite cardFrontSprite,Sprite cardBackSprite)
        {
            m_DisplayFront = true;
            m_CardViewer.gameObject.SetActive(true);
            m_Image.sprite = cardFrontSprite;
            m_FrontSprite = cardFrontSprite;
            m_BackSprite = cardBackSprite;
            DisplaySwap(cardBackSprite);
        }

        public void Swap()
        {
            m_Image.sprite = m_DisplayFront ? m_BackSprite : m_FrontSprite;
            m_DisplayFront = !m_DisplayFront;
        }

        private void DisplaySwap(bool enableSwap)
        {
            m_SwapButton.gameObject.SetActive(enableSwap);        
        }

        public void Hide()
        {
            m_CardViewer.gameObject.SetActive(false);
        }
    }
}