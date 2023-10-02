using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class CardInDeckPointer : ButtonPointer
    {
        [SerializeField] private Image m_CardSprite = null;
        [SerializeField] private TMP_Text m_CardCount = null;

        public void Initialize(Sprite cardSprite, int cardCount)
        {
            m_CardSprite.sprite = cardSprite;
            m_CardCount.text = cardCount.ToString();
        }
        protected override void OnExit()
        {
            return;
        }

        public override void OnRightClick()
        {
            GlobalCanvas.Instance.DisplayCardViewer(m_CardSprite.sprite);
        }

        public override void OnLeftClick()
        {
            
        }
    }
}