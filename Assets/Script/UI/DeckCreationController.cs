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
    }
}