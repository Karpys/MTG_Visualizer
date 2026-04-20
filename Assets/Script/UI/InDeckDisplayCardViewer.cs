namespace Script.UI
{
    using Manager;

    public class InDeckDisplayCardViewer : DisplayCardViewer
    {
        private DeckGestionController m_Controller = null;
        private CardDisplayData m_Data;
        private string m_CardId;
        public void DisplayInDeckCard(DeckGestionController controller,CardDisplayData data,string cardId)
        {
            m_Data = data;
            m_Controller = controller;
            m_CardId = cardId;
            Display(data.m_FrontSprite, data.m_BackSprite);
        }
        public void AssignAsCommander()
        {
            m_Controller.AssignAsCommander(m_CardId);
        }
    }
}