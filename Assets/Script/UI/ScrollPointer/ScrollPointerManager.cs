using System;
using MTG;
using UnityEngine;

namespace Script.UI.ScrollPointer
{
    public class ScrollPointerManager : SingletonMonoBehaviour<ScrollPointerManager>
    {
        private ScrollPointer m_CurrentScroll = null;

        public void SetCurrentScroll(ScrollPointer scroll)
        {
            m_CurrentScroll = scroll;
        }

        private void Update()
        {
            float mouseDelta = Input.mouseScrollDelta.y;

            if (mouseDelta != 0 && m_CurrentScroll != null && m_CurrentScroll.PointerUp)
            {
                if (mouseDelta > 0)
                {
                    m_CurrentScroll.MoveUp(mouseDelta);
                }
                else
                {
                    m_CurrentScroll.MoveDown(Mathf.Abs(mouseDelta));
                }
            }
        }
    }
}