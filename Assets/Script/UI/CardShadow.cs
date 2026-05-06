namespace Script.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class CardShadow : MonoBehaviour
    {
        [SerializeField] private Image m_Image = null;

        private CardInLibraryPointer m_CardInLibrary = null;
        
        public void Initialize(Sprite sprite, CardInLibraryPointer cardInLibrary)
        {
            m_CardInLibrary = cardInLibrary;
            m_Image.sprite = sprite;
        }
    }
}