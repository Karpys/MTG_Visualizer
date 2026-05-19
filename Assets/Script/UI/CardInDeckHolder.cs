using TMPro;
using UnityEngine;

namespace Script.UI
{
    using Helper;

    public class CardInDeckHolder : BaseCardInDeckUI, IPosition
    {
        [SerializeField] private TMP_Text m_CardCount = null;
        [SerializeField] private CardInDeckDraggable m_CardInDeckDraggable = null;

        public Vector3 Position => transform.position;
        public IUIDraggable Draggable => m_CardInDeckDraggable;

        public void Initialize(int cardCount)
        {
            m_CardCount.text = cardCount.ToString();
        }

        public void ChangeCardCount(int count)
        {
            int newCount = m_Controller.ChangeCardCount(m_CardId, count);
            UpdateCoutUI(newCount);
        }

        public void UpdateCoutUI(int cardCountCount)
        {
            m_CardCount.text = cardCountCount.ToString();
        }

        public void DisplayCard()
        {
            m_Controller.DisplayCardInDeck(m_CardDisplayData,m_CardId);
        }
    }
}