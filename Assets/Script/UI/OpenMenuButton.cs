using UnityEngine;

namespace Script.UI
{
    public class OpenMenuButton : ButtonPointer
    {
        [SerializeField] private GlobalCanvas m_GlobalCanvas = null;
        [SerializeField] private MenuType m_MenuType = MenuType.MainMenu;
        protected override void OnExit()
        {
            return;
        }

        public override void OnClick()
        {
            m_GlobalCanvas.OpenMenu(m_MenuType);
        }
    }
}