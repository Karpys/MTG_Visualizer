using System.Collections.Generic;
using Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI
{
    using System.IO;

    public class DeckDefinedContainer : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_DeckName = null;
        [SerializeField] private TMP_Text m_DeckType = null;
        [SerializeField] private Image m_DeckBackgroundImage = null;

        private DeckData m_DeckData;
        private DeckCreationController m_DeckCreationController;

        public void Initialize(DeckData data,DeckCreationController controller)
        {
            m_DeckData = data;
            m_DeckName.text = m_DeckData.DeckName;
            m_DeckType.text = m_DeckData.DeckType.ToString();
            m_DeckBackgroundImage.sprite = m_DeckData.DeckBackCard;
            m_DeckCreationController = controller;
        }

        public void OpenEditScene()
        {
            GlobalCanvas.Instance.SetDeckGestionDeckData(m_DeckData);
            GlobalCanvas.Instance.OpenMenu(MenuType.Deck_Gestion);
        }

        public void OpenVisualizerScene()
        {
            DeckDataHolder.DeckData = m_DeckData;
            SceneManager.LoadScene(1);
        }

        public void DeleteDeck()
        {
            string deckPath = CardFileHelper.GetDeckPath() + m_DeckData.DeckName + ".deck";
            File.Delete(deckPath);
            m_DeckCreationController.OnDeckDelete();
        }
    }


    public struct DeckData
    {
        public string DeckName;
        public string DeckBackPath;
        public DeckType DeckType;
        public Sprite DeckBackCard;
        public List<CardCount> DeckCards;

        public DeckData(string deckName, DeckType deckType, string deckBackPath, Sprite deckBackCard,
            List<CardCount> deckCards)
        {
            DeckName = deckName;
            DeckBackPath = deckBackPath;
            DeckType = deckType;
            DeckBackCard = deckBackCard;
            DeckCards = deckCards;
        }

        public string[] ToFile()
        {
            string[] fileLines = new string[4 + DeckCards.Count];

            fileLines[0] = DeckName;
            fileLines[1] = DeckBackPath;
            fileLines[2] = ((int)DeckType).ToString();

            for (int i = 0; i < DeckCards.Count; i++)
            {
                fileLines[i + 4] = DeckCards[i].Count + "|" + DeckCards[i].CardId;
            }

            return fileLines;
        }
    }

    public struct CardCount
    {
        public int Count;
        public string CardId;

        public CardCount(int count, string id)
        {
            Count = count;
            CardId = id;
        }
    }

    public enum DeckType
    {
        Standard = 0,
        Commander = 1,
    }
}