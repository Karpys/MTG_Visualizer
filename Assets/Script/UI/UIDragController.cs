namespace Script.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    public class UIDragController : MonoBehaviour
    {
        [SerializeField] private float m_RangeSelect = 0;
        [SerializeField] private float m_RangeStartDrag = 0;

        private List<IUIDraggable> m_UIDraggables = new List<IUIDraggable>();

        private bool m_InDrag = false;
        private Vector2 m_StartDragPosition = Vector2.zero;
        private IUIDraggable m_CurrentDrag = null;
        
        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                TrySelectUIDraggable();
            }else if (Input.GetMouseButtonUp(0))
            {
                TryReleaseDraggable();
            }

            if (m_CurrentDrag != null)
            {

                if (!m_InDrag)
                {
                    TryStartDrag();
                }
                else
                {
                    m_CurrentDrag.OnDragUpdate(Input.mousePosition);
                }
            }
        }

        private void TrySelectUIDraggable()
        {
            if(m_UIDraggables.Count <= 0)
                return;
            
            Vector2 mousePosition = Input.mousePosition;
            m_CurrentDrag = m_UIDraggables[0];
            float closestDist = Vector2.Distance(mousePosition, m_UIDraggables[0].RectTransform.position);

            foreach (IUIDraggable draggable in m_UIDraggables)
            {
                if(draggable.RectTransform == null)
                    continue;
                
                float dist = Vector2.Distance(mousePosition, draggable.RectTransform.position); 
                
                if (dist <= closestDist)
                {
                    m_CurrentDrag = draggable;
                    closestDist = dist;
                }
            }

            if (closestDist > m_RangeSelect)
            {
                m_CurrentDrag = null;
            }
            else
            {
                m_StartDragPosition = mousePosition;
                m_InDrag = false;
            }
        }

        private void TryStartDrag()
        {
            Vector2 mousePosition = Input.mousePosition;

            if (Vector2.Distance(mousePosition, m_StartDragPosition) >= m_RangeStartDrag)
            {
                m_CurrentDrag.OnDragSelect();
                m_CurrentDrag.OnDragUpdate(Input.mousePosition);
                m_InDrag = true;
            }
        }

        private void TryReleaseDraggable()
        {
            if (m_CurrentDrag == null)
                return;
            
            m_CurrentDrag.ReleaseDrag();
            m_CurrentDrag = null;
            m_InDrag = false;
        }

        public void SetDraggable(List<IUIDraggable> draggables)
        {
            m_UIDraggables = draggables;
        }
    }
}