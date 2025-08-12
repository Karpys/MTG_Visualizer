using System;
using UnityEngine;

namespace Script.Behaviour
{
    public class PositionAttach : MonoBehaviour
    {
        private Transform m_Attach = null;
        
        public void SetAttach(Transform attach)
        {
            m_Attach = attach;
        }
        
        private void LateUpdate()
        {
            if(m_Attach == null)
                return;
            transform.position = m_Attach.position;
        }
    }
}