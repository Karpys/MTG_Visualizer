using DG.Tweening;
using UnityEngine;
namespace MTG
{
    public class JetonHolder : Holder
    {
        public override void RemoveCard(CardHolder card)
        {
        }
        protected override void UpdateCardPosition(CardHolder card, int index)
        {
            card.transform.DOKill();
            card.ResetRotation();
            card.transform.position = m_StartTransform.position;
        }

        protected override Vector3 GetPosition(CardHolder card,int index)
        {
            return Vector3.zero;
        }
    }
}