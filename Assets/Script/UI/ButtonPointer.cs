namespace Script.UI
{
    public abstract class ButtonPointer : UIPointer
    {
        protected override void OnEnter()
        {
            ButtonPointerManager.Instance.SetCurrentButton(this);   
        }

        public abstract void OnClick();
    }
}