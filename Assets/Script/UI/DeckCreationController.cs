using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ookii.Dialogs;
using Script.Helper;
using TMPro;
using UnityEngine;

namespace Script.UI
{
    public class DeckCreationController : MonoBehaviour
    {
        [SerializeField] private Transform m_DeckCreationPopup = null;
        [SerializeField] private Transform m_DeckGridRoot = null;
        [SerializeField] private DeckDefinedContainer m_DeckDefinedContainer = null;

        [Header("References")] 
        [SerializeField] private TMP_Text m_SelectBackCardText = null;
        [SerializeField] private TMP_InputField m_DeckNameText = null;
        
        private string m_CurrentBackCardPath = String.Empty;

        private List<DeckDefinedContainer> m_DeckDefinedContainers = new List<DeckDefinedContainer>();
        private void OnEnable()
        {
            ClearOldContainers();
            CreateExistentDeckContainers();
        }

        private void CreateExistentDeckContainers()
        {
            string[] decks = Directory.GetFiles(FileHelper.GetDeckPath()).Where(s => s.Contains(".dck")).ToArray();

            for (int i = 0; i < decks.Length; i++)
            {
                DeckDefinedContainer container = Instantiate(m_DeckDefinedContainer,m_DeckGridRoot);
                string[] deckLines = File.ReadAllLines(decks[i]);
                DeckData deckData = new DeckData(deckLines[0], (DeckType) int.Parse(deckLines[2]));
                container.Initialize(deckData);
                m_DeckDefinedContainers.Add(container);
            }
        }

        private void ClearOldContainers()
        {
            for (int i = 0; i < m_DeckDefinedContainers.Count; i++)
            {
                Destroy(m_DeckDefinedContainers[i].gameObject);
            }
            
            m_DeckDefinedContainers.Clear();
        }

        public void OpenDeckCreationPopup()
        {
            m_DeckCreationPopup.gameObject.SetActive(true);
        }

        public void SelectBackCard()
        {
            VistaOpenFileDialog openFile = new VistaOpenFileDialog();
            openFile.Filter = "Images (*.jpg)|*.jpg";
            
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                m_CurrentBackCardPath = openFile.FileName;
                m_SelectBackCardText.text = m_CurrentBackCardPath.ToFileName();
            }
            else
            {
                Debug.LogError("Select a correct file format");
            }
        }

        public void CheckCreateDeck()
        {
            if (CanCreateDeck())
            {
                CreateDeck();
            }
            else
            {
                Debug.Log("Cannot create deck");
            }
        }

        private void CreateDeck()
        {
            Debug.Log("Create deck => Open Deck Popup with new deck");
        }

        private bool CanCreateDeck()
        {
            return m_CurrentBackCardPath != String.Empty && m_DeckNameText.text != String.Empty;
        }
    }
}