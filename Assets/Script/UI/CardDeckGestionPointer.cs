using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class CardDeckGestionPointer : ButtonPointer
    {
        [SerializeField] private Image m_Image = null;
        protected override void OnExit()
        {
            return;
        }

        public override void OnRightClick()
        {
            GlobalCanvas.Instance.DisplayCardViewer(m_Image.sprite);
        }

        public override void OnLeftClick()
        {
            return;
        }
    }
}