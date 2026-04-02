namespace Script.UI
{
    using UnityEngine;

    public class FiltersController : MonoBehaviour
    {
        [Header("Filters")] 
        [SerializeField] private Transform m_Filters = null;
        
        public void OpenFilters()
        {
            m_Filters.gameObject.SetActive(true);
        }
        
        public void CloseFilters()
        {
            m_Filters.gameObject.SetActive(false);
        }
    }
}