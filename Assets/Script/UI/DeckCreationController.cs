using System;
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

        [Header("References")] 
        [SerializeField] private TMP_Text m_SelectBackCardText = null;
        
        private string m_CurrentBackCardPath = String.Empty;
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
    }
}