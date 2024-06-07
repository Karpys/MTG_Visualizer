using System.IO;
using Script.Behaviour;
using UnityEngine;

namespace Script.UI
{
    public class DownloadCardButton : MonoBehaviour
    {
        [SerializeField] private Vector2Int m_BorderSize = new Vector2Int(10,10);
        [SerializeField] private Color m_BorderColor = Color.black;
        [SerializeField] private PositionAttach m_Attach = null;
        private PreviewCardData m_CardData;
        
        public void SetCardData(PreviewCardData cardData)
        {
            m_CardData = cardData;
        }

        public void DownloadCard()
        {
            CardFileHelper.DownloadToLibrary(m_CardData,m_BorderColor,m_BorderSize);
        }

        public void SetAttach(Transform attach)
        {
            m_Attach.SetAttach(attach);
        }
    }
}