using DG.Tweening;
using Manager;
using UnityEngine;

namespace MTG
{
    public class GraveyardHolder : Holder
    {
        public override void RemoveCard(CardHolder card)
        {
            base.RemoveCard(card);
            CardUIDisplay.Instance.UpdateDisplay(m_Cards,CardState.Graveyard);
        }
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