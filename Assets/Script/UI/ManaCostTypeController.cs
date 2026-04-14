namespace Script.UI
{
    using System;
    using UnityEngine;

    public class ManaCostTypeController : MonoBehaviour
    {
        [SerializeField] private ManaCostTypeHolderButton[] m_HolderButtons = null;
        [SerializeField] private ManaSymbolSearchType m_SearchType = ManaSymbolSearchType.Same;
        public ManaSymbolSearchType SearchType => m_SearchType;
        
        public void SetFilter(ManaCostTypeHolderButton costHolder, ManaSymbolSearchType filter)
        {
            m_SearchType = filter;

            foreach (var holder in m_HolderButtons)
            {
                if (holder != costHolder)
                {
                    holder.DisableOutline();
                }
            }
        }
    }
}