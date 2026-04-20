namespace Script.Manager
{
    using UnityEngine;

    public class DeckGestionContextDisplayer : MonoBehaviour
    {
        [SerializeField] private Transform m_DeckHolder = null;
        [SerializeField] private Transform m_TokenHolder = null;
        [SerializeField] private Transform m_CommanderHolder = null;
        
        public void Display(DeckGestionContext context)
        {
            switch (context)
            {
                case DeckGestionContext.Deck:
                    m_DeckHolder.gameObject.SetActive(true);
                    m_TokenHolder.gameObject.SetActive(false);
                    m_CommanderHolder.gameObject.SetActive(false);
                    break;
                case DeckGestionContext.Token:
                    m_TokenHolder.gameObject.SetActive(true);
                    m_DeckHolder.gameObject.SetActive(false);
                    m_CommanderHolder.gameObject.SetActive(false);
                    break;
                case DeckGestionContext.Commander:
                    m_CommanderHolder.gameObject.SetActive(true);
                    m_DeckHolder.gameObject.SetActive(false);
                    m_TokenHolder.gameObject.SetActive(false);
                    break;
            }
        }
    }
}