using System;
using DG.Tweening;
using Script;
using Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        Jeton,
        CommandZone,
    }
    public class CardHolder:MonoBehaviour
    {
        [SerializeField] private Canvas m_Canvas = null;
        [SerializeField] private Image m_Visual = null;
        [SerializeField] private BoxCollider2D m_Selection = null;
        [SerializeField] private CardState m_State = CardState.Deck;
        [SerializeField] private TextMeshProUGUI m_Counter = null;

        private Sprite m_CardVisual = null;
        private int m_CurrentCount = 0;
        private CardNameData m_CardNameData;
        public BoxCollider2D Selection => m_Selection;
        public CardState State => m_State;
        public Sprite CardVisual => m_CardVisual;
        public CardNameData CardNameData => m_CardNameData;

        public void Initialize(string cardId)
        {
            m_CardNameData = cardId.CardIdToCardNameData();
            m_CardVisual = m_CardNameData.CardPathName.ToCardSprite();
            UpdateVisual(m_State);
            gameObject.name = m_CardNameData.CardName;
        }

        public void Initialize(CardScriptable cardScriptable)
        {
            m_CardVisual = cardScriptable.m_CardVisual;
            UpdateVisual(m_State);
            gameObject.name = cardScriptable.name;
        }
        
        public void Initialize(CardHolder cardHolder)
        {
            m_CardVisual = cardHolder.CardVisual;
            m_State = cardHolder.State;
            UpdateVisual(m_State);
            gameObject.name = cardHolder.name;
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
            Sprite forceSprite = null;
            switch (state)
            {
                case CardState.Deck:
                    newSprite = HolderManager.Instance.DefaultSprite;
                    m_Visual.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    break;
                case CardState.Hand:
                    if(!HolderManager.Instance.DisplayHandCards)
                        forceSprite = HolderManager.Instance.DefaultSprite;
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
                case CardState.Jeton:
                    m_Visual.transform.localScale = Vector3.zero;
                    goto default;
                default:
                    newSprite = m_CardVisual;
                    ResetRotation();
                    break;
            }

            if (forceSprite)
            {
                m_Visual.sprite = forceSprite;
            }
            else
            {
                m_Visual.sprite = newSprite;
            }
        }

        public void ResetRotation()
        {
            transform.DORotate(new Vector3(0, 0, 0), 0.15f);
        }

        public void RotateCard()
        {
            if (transform.eulerAngles == new Vector3(0, 0, 270))
            {
                ResetRotation();
                return;
            }
            transform.DORotate(new Vector3(0, 0, -90), 0.15f);
        }

        public void SetSpritePriority(int newPrio)
        {
            m_Canvas.sortingOrder = newPrio;
        }

        public int GetPriority()
        {
            return m_Canvas.sortingOrder;
        }

        public void ChangeCounter(int add)
        {
            m_CurrentCount += add;
            UpdateVisualCount();
        }
        private void UpdateVisualCount()
        {
            string prefix = String.Empty;
            if (m_CurrentCount > 0)
            {
                prefix = "+";
            }
            else if(m_CurrentCount < 0)
            {
                prefix = " ";
            }
            else
            {
                m_Counter.text = "";
                return;
            }

            m_Counter.text = prefix + m_CurrentCount;
        }
    }
}