using UnityEngine;
using UnityEngine.UI;

namespace MTG
{
    public class CardHelpDisplay : SingletonMonoBehaviour<CardHelpDisplay>
    {
        [SerializeField] private Image m_Preview = null;
        [SerializeField] private Transform m_PreviewTransform = null;
        
        public void DisplayPreviewCard(CardHolder card)
        {
            m_PreviewTransform.gameObject.SetActive(true);
            m_Preview.sprite = card.CardVisual;
        }

        public void RemoveDisplay()
        {
            m_PreviewTransform.gameObject.SetActive(false);
        }
    }
}