using DG.Tweening;
using UnityEngine;

namespace MTG
{
    public class GraveyardHolder : Holder
    {
        protected override void UpdateCardPosition(CardHolder card, int index)
        {
            card.transform.DOKill();
            card.transform.position = m_StartTransform.position;
        }

        protected override Vector3 GetPosition(CardHolder card,int index)
        {
            return Vector3.zero;
        }
    }
}