namespace Script.UI
{
    using UnityEngine;

    public interface IUIDraggable
    {
        RectTransform RectTransform { get; }
        public void OnDragUpdate(Vector2 mousePosition);
        public void OnDragSelect();
        public void ReleaseDrag();
    }
}