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

        [SerializeField] private CardState m_State = CardState.Deck;


        private Sprite m_CardVisual = null;
        public void Initialize(CardScriptable cardScriptable)
        {
            m_CardVisual = cardScriptable.m_CardVisual;
            UpdateVisual(m_State);
        }

        public void UpdateState(CardState state)
        {
            m_State = state;
            UpdateVisual(state);
        }
        

        private void UpdateVisual(CardState state)
        {
            Sprite newSprite = null;
            switch (state)
            {
                case CardState.Deck:
                    newSprite = GameManager.Instance.DeckHolder.Deck.m_CardBackSprite;
                    m_Visual.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    break;
                default:
                    newSprite = m_CardVisual;
                    break;
            }

            m_Visual.sprite = newSprite;
        }
    }
}