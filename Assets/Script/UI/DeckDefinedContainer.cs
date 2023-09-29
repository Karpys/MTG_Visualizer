using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class DeckDefinedContainer : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_DeckName = null;
        [SerializeField] private TMP_Text m_DeckType = null;
        [SerializeField] private Image m_DeckBackgroundImage = null;

        private DeckData m_DeckData;

        public void Initialize(DeckData data)
        {
            m_DeckData = data;
            m_DeckName.text = m_DeckData.DeckName;
            m_DeckType.text = m_DeckData.DeckType.ToString();
            m_DeckBackgroundImage.sprite = m_DeckData.DeckBackCard;
        }
    }


    public struct DeckData
    {
        public string DeckName;
        public DeckType DeckType;
        public Sprite DeckBackCard;

        public DeckData(string deckName, DeckType deckType,Sprite deckBackCard)
        {
            DeckName = deckName;
            DeckType = deckType;
            DeckBackCard = deckBackCard;
        }
    }

    public enum DeckType
    {
        Standard = 0,
        Commander = 1,
    }
}