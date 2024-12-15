namespace Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;

    public class CardTypeFilter : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown m_Dropdown = null;

        private void Awake()
        {
            InitializeCardTypeFilter();
        }

        private void InitializeCardTypeFilter()
        {
            List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
            CardType[] types = ((CardType[])Enum.GetValues(typeof(CardType))).Where(e => (int)e >= 0).ToArray();

            foreach (CardType cardType in types)
            {
                optionDatas.Add(new TMP_Dropdown.OptionData(cardType.ToString()));
            }
            
            m_Dropdown.AddOptions(optionDatas);
        }

        public string GetFilter()
        {
            string typeFilter = GetTypeFilter();

            if (typeFilter == "")
                return "";

            return "+type:" + typeFilter;
        }
        
        private string GetTypeFilter()
        {
            CardType cardTypeSelected = (CardType)Enum.Parse(typeof(CardType), m_Dropdown.options[m_Dropdown.value].text);

            switch (cardTypeSelected)
            {
                case CardType.Other:
                    return "";
                case CardType.Null:
                    return "";
                case CardType.Creature:
                    return "creature";
                case CardType.Land:
                    return "land";
                case CardType.Enchantment:
                    return "enchantment";
                case CardType.Planeswalker:
                    return "planeswalker";
                case CardType.Instant:
                    return "instant";
                case CardType.Sorcery:
                    return "sorcery";
                case CardType.Battle:
                    return "battle";
                case CardType.Artifact:
                    return "artifact";
                default:
                    return "";
            }

            return "";
        }
    }
}