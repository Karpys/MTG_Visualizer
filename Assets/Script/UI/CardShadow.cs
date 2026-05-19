namespace Script.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class CardShadow : MonoBehaviour
    {
        [SerializeField] private Image m_Image = null;

        public void Initialize(Sprite sprite)
        {
            m_Image.sprite = sprite;
        }
    }
}