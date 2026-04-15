namespace Script.UI
{
    using UnityEngine;
    using UnityEngine.Events;

    public class EventButtonPointer : ButtonPointer
    {
        [SerializeField] private UnityEvent m_RightClickUnityAction = null;
        [SerializeField] private UnityEvent m_LeftClickUnityAction = null;
        protected override void OnExit()
        {
            
        }

        public override void OnRightClick()
        {
            m_RightClickUnityAction?.Invoke();
        }

        public override void OnLeftClick()
        {
            m_LeftClickUnityAction?.Invoke();
        }
    }
}