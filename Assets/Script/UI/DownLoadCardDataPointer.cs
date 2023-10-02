using UnityEngine;

namespace Script.UI
{
    public class DownLoadCardDataPointer : ButtonPointer
    {
        [SerializeField] private Transform m_ButtonPosition = null;
        
        private PreviewCardData m_CardData;
        private DownloadCardButton m_ButtonTransform = null;
        
        public PreviewCardData CardData => m_CardData;

        public void Initialize(PreviewCardData cardData, DownloadCardButton downloadButtonTransform)
        {
            m_CardData = cardData;
            m_ButtonTransform = downloadButtonTransform;
        }
        public override void OnRightClick()
        {
            GlobalCanvas.Instance.DisplayCardViewer(CardData.sprite);
        }

        public override void OnLeftClick()
        {
            PlaceDownloadButton();
        }

        private void PlaceDownloadButton()
        {
            m_ButtonTransform.gameObject.SetActive(true);
            m_ButtonTransform.transform.position = m_ButtonPosition.position;
            m_ButtonTransform.SetCardData(m_CardData);
            m_ButtonTransform.SetAttach(m_ButtonPosition);
        }

        protected override void OnExit()
        {
        }
    }
}