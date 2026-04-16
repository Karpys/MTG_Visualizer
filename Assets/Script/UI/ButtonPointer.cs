namespace Script.UI
{
    public static class UpdateCardData
    {
        public static void Update()
        {
        }
    }
    
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