namespace MTG
{
    using UnityEngine;

    public class HelperMenu : MonoBehaviour
    {
        [SerializeField] private HolderManager m_HolderManager = null;
        [SerializeField] private Transform m_Container = null;
        [SerializeField] private CardUIDisplay m_CardUIDisplay = null;
        
        private bool m_IsOpen = false;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
                TryOpen();
        }

        public void TryOpen()
        {
            if (m_IsOpen)
            {
                Hide();
            }
            else
            {
                Open();
            }

            m_IsOpen = !m_IsOpen;
        }

        public void Hide()
        {
            m_Container.gameObject.SetActive(false);
        }

        private void Open()
        {
            m_Container.gameObject.SetActive(true);
        }

        public void ShuffleDeck()
        {
            m_HolderManager.DeckHolder.Shuffle();
            m_CardUIDisplay.UpdateDisplay(m_HolderManager.GetHolder(m_HolderManager.GetState(KeyCode.D)).Cards, m_HolderManager.GetState(KeyCode.D));
        }
    }
}