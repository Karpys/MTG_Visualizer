using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public abstract class UIPointer : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        private bool m_PointerUp = false;

        public bool PointerUp => m_PointerUp;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            m_PointerUp = true;
            OnEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_PointerUp = false;
            OnExit();
        }

        protected abstract void OnEnter();
        protected abstract void OnExit();
    }
}