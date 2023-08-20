using System;
using MTG;
using UnityEngine;

namespace Script.UI
{
    public class ButtonPointerManager : SingletonMonoBehaviour<ButtonPointerManager>
    {
        private ButtonPointer m_CurrentPointer = null;
        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_CurrentPointer != null && m_CurrentPointer.PointerUp)
                {
                    m_CurrentPointer.OnClick();
                }
            }
        }

        public void SetCurrentButton(ButtonPointer buttonPointer)
        {
            m_CurrentPointer = buttonPointer;
        }

        public void Clear(ButtonPointer buttonPointer)
        {
            if (m_CurrentPointer == buttonPointer)
                m_CurrentPointer = null;
        }
    }
}