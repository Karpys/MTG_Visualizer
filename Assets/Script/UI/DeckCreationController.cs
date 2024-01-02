using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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
        [SerializeField] private TMP_Dropdown m_DeckTypeDropDown = null;
        
        private string m_CurrentBackCardPath = String.Empty;

        private List<DeckDefinedContainer> m_DeckDefinedContainers = new List<DeckDefinedContainer>();
        private void OnEnable()
        {
            UpdateContainers();
        }

        private void UpdateContainers()
        {
            ClearOldContainers();
            CreateExistentDeckContainers();
        }

        private void CreateExistentDeckContainers()
        {
            string[] decks = Directory.GetFiles(CardFileHelper.GetDeckPath()).Where(s => s.Contains(".dck")).ToArray();

            for (int i = 0; i < decks.Length; i++)
            {
                DeckDefinedContainer container = Instantiate(m_DeckDefinedContainer,m_DeckGridRoot);
                string[] deckInfo = File.ReadAllLines(decks[i]);
                DeckData deckData = deckInfo.ToDeckData();
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

        private void CloseDeckCreationPopup()
        {
            m_DeckCreationPopup.gameObject.SetActive(false);
            //Todo : Reset to default state//
        }

        public void SelectBackCard()
        {
            string filePath = FileHelper.GetFilePath("(*.jpg)|*.jpg");
            
            if (filePath!=String.Empty)
            {
                m_CurrentBackCardPath = filePath;
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
            CloseDeckCreationPopup();
            CreateBackCardImage();
            CreateDeckFile(m_DeckNameText.text,m_DeckTypeDropDown.value,m_CurrentBackCardPath.ToFileName());
            UpdateContainers();
        }

        private void CreateBackCardImage()
        {
            byte[] backCardImage = File.ReadAllBytes(m_CurrentBackCardPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(backCardImage);
            texture = texture.ResizeTexture(488, 680);
            
            string path = CardFileHelper.GetDeckBackCardPath()+m_CurrentBackCardPath.ToFileName();
            File.WriteAllBytes(path,texture.EncodeToJPG());
        }

        private void CreateDeckFile(string deckName,int deckType,string deckBackFileName)
        {
            string path = CardFileHelper.GetDeckPath() + deckName + ".dck";
            string[] deckData = new string[3];
            deckData[0] = deckName;
            deckData[1] = deckBackFileName;
            deckData[2] = deckType.ToString();
            File.WriteAllLines(path,deckData);
        }

        private bool CanCreateDeck()
        {
            return m_CurrentBackCardPath != String.Empty && m_DeckNameText.text != String.Empty;
        }
    }
}