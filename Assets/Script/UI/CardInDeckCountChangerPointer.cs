using UnityEngine;

namespace Script.UI
{
    public class CardInDeckCountChangerPointer : ButtonPointer
    {
        [SerializeField] private CardInDeckHolder m_CardInDeckPointer = null;
        [SerializeField] private int m_ChangeCount = 0;
        protected override void OnExit()
        {
            return;
        }

        public override void OnRightClick()
        {
            return;
        }

        public override void OnLeftClick()
        {
            m_CardInDeckPointer.ChangeCardCount(m_ChangeCount);
        }
    }
}