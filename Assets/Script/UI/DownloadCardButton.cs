using System.IO;
using UnityEngine;

namespace Script.UI
{
    public class DownloadCardButton : MonoBehaviour
    {
        private CardData m_CardData;
        
        public void SetCardData(CardData cardData)
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
    }
}