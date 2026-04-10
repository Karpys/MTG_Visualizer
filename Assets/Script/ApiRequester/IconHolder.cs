namespace Script
{
    using UnityEngine;
    using UnityEngine.UI;

    public class IconHolder : MonoBehaviour
    {
        [SerializeField] private Image m_Image = null;

        private ManaSymbol m_ManaSymbol = ManaSymbol.Black;
        public ManaSymbol ManaSymbol => m_ManaSymbol;

        public void Initialize(Sprite sprite, ManaSymbol manaSymbol)
        {
            m_Image.sprite = sprite;
            m_ManaSymbol = manaSymbol;
        }
    }
}