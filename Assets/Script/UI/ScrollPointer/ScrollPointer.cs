using DG.Tweening;
using Script.Helper;
using UnityEngine;

namespace Script.UI.ScrollPointer
{
    public class ScrollPointer : UIPointer
    {
        [SerializeField] private Transform m_ScrollContent = null;
        [SerializeField] private Vector2 m_ScrollClamp = Vector2.zero;
        [SerializeField] private float m_MoveForce = 0;
        protected override void OnEnter()
        {
            ScrollPointerManager.Instance.SetCurrentScroll(this);
        }

        protected override void OnExit()
        {
        }

        public void MoveUp(float force)
        {
            var position = m_ScrollContent.transform.localPosition;
            float targetY = Mathf.Min(position.y + m_MoveForce * force, m_ScrollClamp.y);
            m_ScrollContent.transform.DOLocalMove(position.Y(targetY),0.5f);
        }

        public void MoveDown(float force)
        {
            var position = m_ScrollContent.transform.localPosition;
            float targetY = Mathf.Max(position.y - m_MoveForce * force, m_ScrollClamp.x);
            m_ScrollContent.transform.DOLocalMove(position.Y(targetY),0.5f);
        }
        
        public void SetMaxUpClamp(float clampUp)
        {
            m_ScrollClamp.y = clampUp;
        }

        public void SetMaxDownClamp(float clampDown)
        {
            m_ScrollClamp.x = clampDown;
        }

        public void SetClamp(Vector2 clamp)
        {
            m_ScrollClamp = clamp;
        }
    }
}