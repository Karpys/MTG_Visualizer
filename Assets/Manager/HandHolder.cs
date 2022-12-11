using DG.Tweening;
using UnityEngine;

namespace MTG
{
    public class HandHolder : Holder
    {
        [SerializeField] private float m_HorizontalSpacing = 1f;
        [SerializeField] private float m_VerticalSpacing = 1f;
        protected override void UpdateCardPosition(Transform card, int index)
        {
            card.DOMove(GetPosition(index), 1f);
        }

        protected override Vector3 GetPosition(int index)
        {
            Vector3 newPos = m_StartTransform.position;
            newPos.x += index * m_HorizontalSpacing;
            return newPos;
        }
    }
}