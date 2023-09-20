using System.Windows.Forms;
using Ookii.Dialogs;
using UnityEngine;

namespace Script.UI
{
    public class DeckCreationController : MonoBehaviour
    {
        [SerializeField] private Transform m_DeckCreationPopup = null;
        
        public void OpenDeckCreationPopup()
        {
            m_DeckCreationPopup.gameObject.SetActive(true);
            
        }

        public void SelectBackCard()
        {
            VistaOpenFileDialog openFile = new VistaOpenFileDialog();
            openFile.Filter = "All Files (*.*)|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                Debug.Log("Coucou");
            }
            //Open Folder//
            //Select a jpg file//
            //Close Folder//
        }
    }
}