namespace Script.UI
{
    public abstract class ButtonPointer : UIPointer
    {
        protected override void OnEnter()
        {
            ButtonPointerManager.Instance.SetCurrentButton(this);   
        }

        public abstract void OnRightClick();
        public abstract void OnLeftClick();

        private void OnDisable()
        {
            if(ButtonPointerManager.Instance)
                ButtonPointerManager.Instance.Clear(this);
        }
    }
}