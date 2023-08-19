using UnityEngine;

namespace Script.UI
{
    public class DownLoadCardDataPointer : ButtonPointer
    {
        [SerializeField] private Transform m_ButtonPosition = null;
        
        private CardData m_CardData;
        private DownloadCardButton m_ButtonTransform = null;
        
        public CardData CardData => m_CardData;

        public void Initialize(CardData cardData, DownloadCardButton downloadButtonTransform)
        {
            m_CardData = cardData;
            m_ButtonTransform = downloadButtonTransform;
        }
        public override void OnClick()
        {
            PlaceDownloadButton();
        }

        private void PlaceDownloadButton()
        {
            m_ButtonTransform.gameObject.SetActive(true);
            m_ButtonTransform.transform.position = m_ButtonPosition.position;
            m_ButtonTransform.SetCardData(m_CardData);
        }

        protected override void OnExit()
        {
        }
    }
}