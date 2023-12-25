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
            Texture2D texture = new Texture2D(488,680);
            texture.SetPixels(m_CardData.sprite.texture.GetPixels());
            SetBorderColor(texture);
            byte[] pixels = texture.EncodeToJPG();
            string filePath = CardFileHelper.GetCardsLibraryPath();
            filePath += m_CardData.cardSaveName+".jpg";
            File.WriteAllBytes(filePath, pixels);
        }
        
        private void SetBorderColor(Texture2D texture)
        {
            int width = texture.width;
            int height = texture.height;
            
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    if (x <= m_BorderSize.x || x >= width - m_BorderSize.x || y <= m_BorderSize.y || y >= height - m_BorderSize.y)
                    {
                        texture.SetPixel(x,y,m_BorderColor);
                    }
                }
            }
            
            texture.Apply();
        }

        public void SetAttach(Transform attach)
        {
            m_Attach.SetAttach(attach);
        }
    }
}