using System.IO;
using Script.Behaviour;
using UnityEngine;

namespace Script.UI
{
    public class DownloadCardButton : MonoBehaviour
    {
        [SerializeField] private PositionAttach m_Attach = null;
        private PreviewCardData m_CardData;
        
        public void SetCardData(PreviewCardData cardData)
        {
            m_CardData = cardData;
        }

        public void DownloadCard()
        {
            byte[] pixels = m_CardData.sprite.texture.EncodeToJPG();
            string filePath = FileHelper.GetCardsLibraryPath();
            filePath += m_CardData.cardSaveName+".jpg";
            File.WriteAllBytes(filePath, pixels);
        }

        public void SetAttach(Transform attach)
        {
            m_Attach.SetAttach(attach);
        }
    }
}