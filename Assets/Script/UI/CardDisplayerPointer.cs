using UnityEngine;

namespace Script.UI
{
    public class CardDisplayerPointer : ButtonPointer
    {
        [SerializeField] private CardInDeckHolder m_CardInDeckHolder = null;
        protected override void OnExit()
        {
            
        }

        public override void OnRightClick()
        {
            m_CardInDeckHolder.DisplayCard();
        }

        public override void OnLeftClick()
        {
        }
    }
}