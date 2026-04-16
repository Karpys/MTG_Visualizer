namespace Script.UI
{
    using System.Collections.Generic;
    using MTG;
    using Manager;
    using Widget;
    using UnityEngine;
    public class GlobalCanvas : SingletonMonoBehaviour<GlobalCanvas>
    {
        [SerializeField] private List<Transform> m_SubCanvas = null;
        [SerializeField] private Transform m_MainMenu = null;

        [SerializeField] private GenericLibrary<Transform, MenuType> m_MenuLibrary = null;

        [Header("References")] 
        [SerializeField] private DeckGestionController m_DeckGestionController = null;

        [Header("Card Viewer")]
        [SerializeField] private DisplayCardViewer m_CardViewer = null;

        private void Awake()
        {
            UpdateCardData.Update();
            m_MenuLibrary.InitializeDictionary();
        }

        public void ReturnMainMenu()
        {
            for (int i = 0; i < m_SubCanvas.Count; i++)
            {
                m_SubCanvas[i].gameObject.SetActive(false);
            }
            
            m_MainMenu.gameObject.SetActive(true);
        }

        public void OpenMenu(MenuType menuType)
        {
            Transform t = m_MenuLibrary.GetViaKey(menuType);
            t.gameObject.SetActive(true);
            m_MainMenu.gameObject.SetActive(false);
        }

        public void DisplayCardViewer(Sprite cardFrontSprite, Sprite cardBackSprite)
        {
            m_CardViewer.Display(cardFrontSprite,cardBackSprite);
        }

        public void CloseCardViewer()
        {
            m_CardViewer.Hide();
        }

        public void SetDeckGestionDeckData(DeckData deckData)
        {
            m_DeckGestionController.SetDeckData(deckData);
        }
    }

    public enum MenuType
    {
        MainMenu,
        Card_Research,
        Deck_Creation,
        Deck_Gestion,
    }
}