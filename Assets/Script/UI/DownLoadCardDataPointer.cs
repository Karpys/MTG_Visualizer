using UnityEngine;

namespace Script.UI
{
    using Newtonsoft.Json.Linq;
    using UnityEngine.UI;

    public class DownLoadCardDataPointer : ButtonPointer
    {
        [SerializeField] private Image m_Image = null;
        [SerializeField] private Transform m_ButtonPosition = null;

        private JObject m_CardObject;
        private ApiCardData m_CardData;
        private DownloadCardButton m_ButtonTransform = null;
        
        public ApiCardData CardData => m_CardData;

        public void Initialize(JObject cardObject,ApiCardData cardData, DownloadCardButton downloadButtonTransform)
        {
            m_CardObject = cardObject;
            m_CardData = cardData;
            m_ButtonTransform = downloadButtonTransform;
            m_Image.sprite = cardData.m_FrontCardSprite;
        }
        
        public override void OnRightClick()
        {
            GlobalCanvas.Instance.DisplayCardViewer(CardData.m_FrontCardSprite, CardData.m_BackCardSprite);
        }

        public override void OnLeftClick()
        {
            PlaceDownloadButton();
        }

        private void PlaceDownloadButton()
        {
            m_ButtonTransform.gameObject.SetActive(true);
            m_ButtonTransform.transform.position = m_ButtonPosition.position;
            m_ButtonTransform.SetCardData(m_CardData,m_CardObject);
            m_ButtonTransform.SetAttach(m_ButtonPosition);
        }

        protected override void OnExit()
        {
        }
    }
}