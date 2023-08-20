using System;

namespace Script.UI
{
    public abstract class ButtonPointer : UIPointer
    {
        protected override void OnEnter()
        {
            ButtonPointerManager.Instance.SetCurrentButton(this);   
        }

        public abstract void OnClick();

        private void OnDisable()
        {
            if(ButtonPointerManager.Instance)
                ButtonPointerManager.Instance.Clear(this);
        }
        
    }
}