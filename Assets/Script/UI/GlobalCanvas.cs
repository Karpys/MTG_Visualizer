using System;
using System.Collections.Generic;
using MTG;
using Script.Manager;
using Script.Widget;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class GlobalCanvas : SingletonMonoBehaviour<GlobalCanvas>
    {
        [SerializeField] private List<Transform> m_SubCanvas = null;
        [SerializeField] private Transform m_MainMenu = null;

        [SerializeField] private GenericLibrary<Transform, MenuType> m_MenuLibrary = null;

        [Header("References")] 
        [SerializeField] private DeckGestionController m_DeckGestionController = null;

        [Header("Card Viewer")]
        [SerializeField] private Transform m_CardViewerTransform = null;
        [SerializeField] private Image m_CardViewerImage = null;

        private void Awake()
        {
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

        public void DisplayCardViewer(Sprite sprite)
        {
            m_CardViewerTransform.gameObject.SetActive(true);
            m_CardViewerImage.sprite = sprite;
        }

        public void CloseCardViewer()
        {
            m_CardViewerTransform.gameObject.SetActive(false);
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