using System;
using System.Collections.Generic;
using MTG;
using Script.Widget;
using UnityEngine;

namespace Script.UI
{
    public class GlobalCanvas : SingletonMonoBehaviour<GlobalCanvas>
    {
        [SerializeField] private List<Transform> m_SubCanvas = null;
        [SerializeField] private Transform m_MainMenu = null;

        [SerializeField] private GenericLibrary<Transform, MenuType> m_MenuLibrary = null;

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
    }

    public enum MenuType
    {
        MainMenu,
        Card_Research,
        Deck_Creation,
    }
}