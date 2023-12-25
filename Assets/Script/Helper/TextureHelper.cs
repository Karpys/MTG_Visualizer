using System.IO;
using UnityEngine;

namespace Script
{
    public static class TextureHelper
    {
        public static Texture2D ResizeTexture(this Texture2D baseTexture, int newWidth, int newHeight)
        {
            Texture2D resizedTexture = new Texture2D(newWidth, newHeight);

            // Resize the texture
            for (int y = 0; y < resizedTexture.height; y++)
            {
                for (int x = 0; x < resizedTexture.width; x++)
                {
                    // Calculate the coordinates in the original texture
                    float xCoord = x * 1.0f / resizedTexture.width * baseTexture.width;
                    float yCoord = y * 1.0f / resizedTexture.height * baseTexture.height;

                    Color color = baseTexture.GetPixel((int)xCoord, (int)yCoord);
                    resizedTexture.SetPixel(x, y, color);
                }
            }

            return resizedTexture;
        }

        public static Sprite ToCardSprite(this string cardPath, int width = 488, int heigth = 680)
        {
            byte[] cardData = File.ReadAllBytes(cardPath);
            Texture2D cardTexture = new Texture2D(width, heigth);
            cardTexture.LoadImage(cardData);
            return Sprite.Create(cardTexture,new Rect(Vector2.zero,new Vector2(width,heigth)),Vector2.zero);
        }
        
        public static Sprite ToCardSprite(this byte[] imageData, int width = 488, int heigth = 680)
        {
            Texture2D cardTexture = new Texture2D(width, heigth);
            cardTexture.LoadImage(imageData);
            return Sprite.Create(cardTexture,new Rect(Vector2.zero,new Vector2(width,heigth)),Vector2.zero);
        }
        
    }
}