using DG.Tweening;
using UnityEngine;

namespace MTG
{
    public enum CardState
    {
        Deck,
        Hand,
        Land,
        Creature,
        Enchantement,
        Exil,
        Graveyard,
        CommandZone,
    }
    public class CardHolder:MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_Visual = null;
        [SerializeField] private BoxCollider2D m_Selection = null;
        [SerializeField] private CardState m_State = CardState.Deck;


        private Sprite m_CardVisual = null;
        public BoxCollider2D Selection => m_Selection;
        public CardState State => m_State;
        public Sprite CardVisual => m_CardVisual;
        public void Initialize(CardScriptable cardScriptable)
        {
            m_CardVisual = cardScriptable.m_CardVisual;
            UpdateVisual(m_State);
        }

        public void UpdateState(CardState state)
        {
            m_State = state;
            SetSpritePriority(0);
            UpdateVisual(state);
        }

        private void UpdateVisual(CardState state)
        {
            Sprite newSprite = null;
            switch (state)
            {
                case CardState.Deck:
                    newSprite = HolderManager.Instance.Deck.m_CardBackSprite;
                    m_Visual.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    break;
                case CardState.Hand:
                    m_Visual.transform.localScale = new Vector3(.7f, .7f, .7f);
                    goto default;
                case CardState.Land:
                    m_Visual.transform.localScale = new Vector3(.4f, .4f, .4f);
                    goto default;
                case CardState.Graveyard:
                    m_Visual.transform.localScale = new Vector3(.6f, .6f, .6f);
                    goto default;
                case CardState.Creature:
                    m_Visual.transform.localScale = new Vector3(.7f, .7f, .7f);
                    goto default;
                case CardState.Exil:
                    m_Visual.transform.localScale = new Vector3(.5f, .5f, .5f);
                    goto default;
                case CardState.Enchantement:
                    m_Visual.transform.localScale = new Vector3(.6f, .6f, .6f);
                    goto default;
                default:
                    newSprite = m_CardVisual;
                    ResetRotation();
                    break;
            }

            m_Visual.sprite = newSprite;
        }

        public void ResetRotation()
        {
            Debug.Log("Reset Rotation");
            transform.DORotate(new Vector3(0, 0, 0), 0.5f);
        }

        public void RotateCard()
        {
            if (transform.eulerAngles == new Vector3(0, 0, 270))
            {
                ResetRotation();
                return;
            }
            transform.DORotate(new Vector3(0, 0, -90), 0.5f);
        }

        public void SetSpritePriority(int newPrio)
        {
            m_Visual.sortingOrder = newPrio;
        }

        public int GetPriority()
        {
            return m_Visual.sortingOrder;
        }
    }
}