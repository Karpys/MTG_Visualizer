namespace Script.Manager
{
    using UI;
    using UnityEngine;

    public class DeckGestionCardViewerController : MonoBehaviour
    {
        [SerializeField] private DisplayCardViewer m_SingleCardViewer = null;
        [SerializeField] private InDeckDisplayCardViewer m_InDeckDisplayCardViewer = null;

        public void DisplaySingle(CardDisplayData data)
        {
            m_SingleCardViewer.Display(data.m_FrontSprite,data.m_BackSprite);
        }

        public void DisplayInDeck(DeckGestionController controller,CardDisplayData data,string cardId)
        {
            m_InDeckDisplayCardViewer.DisplayInDeckCard(controller, data, cardId);
        }
    }
}