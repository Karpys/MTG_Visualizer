using DG.Tweening;
using UnityEngine;

namespace MTG
{
    public class HandHolder : Holder
    {
        [SerializeField] private SocketSender m_Sender = null;

        public override void AddCard(CardHolder card)
        {
            base.AddCard(card);
            m_Sender.RequestWrite();
        }

        public override void RemoveCard(CardHolder card)
        {
            base.RemoveCard(card);
            m_Sender.RequestWrite();
        }
    }
}