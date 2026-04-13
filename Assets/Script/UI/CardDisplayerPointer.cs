using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class CardDisplayerPointer : ButtonPointer
    {
        [SerializeField] private CardInDeckHolder m_CardInDeckHolder = null;
        [SerializeField] private Image m_Image = null;
        protected override void OnExit()
        {
            
        }

        public override void OnRightClick()
        {
            GlobalCanvas.Instance.DisplayCardViewer(m_CardInDeckHolder.DisplayData.m_FrontSprite,m_CardInDeckHolder.DisplayData.m_BackSprite);
        }

        public override void OnLeftClick()
        {
        }
    }
}