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
    }
}