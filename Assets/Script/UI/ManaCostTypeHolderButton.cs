namespace Script.UI
{
    using UnityEngine;

    public class ManaCostTypeHolderButton : MonoBehaviour
    {
        [SerializeField] private ManaCostTypeController m_Controller = null;
        [SerializeField] private Transform m_SelectionOutline = null;
        [SerializeField] private ManaSymbolSearchType m_SearchType = ManaSymbolSearchType.Contains;
        
        public void OnClick()
        {
            m_Controller.SetFilter(this,m_SearchType);       
            m_SelectionOutline.gameObject.SetActive(true);
        }

        public void DisableOutline()
        {
            m_SelectionOutline.gameObject.SetActive(false);
        }
    }
}