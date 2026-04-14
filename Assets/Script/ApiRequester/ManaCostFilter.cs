namespace Script
{
    using System.Collections.Generic;
    using KarpysDev.KarpysUtils;
    using KarpysDev.KarpysUtils.ObjectPooling;
    using UI;
    using UnityEngine;

    public class ManaCostFilter : MonoBehaviour
    {
        [SerializeField] private GenericLibrary<ManaSymbol, Sprite> m_ManaToSpriteLibrary = null;
        [SerializeField] private IconHolder manaCostHolderPrefab = null;
        [SerializeField] private Transform m_IconHolder = null;
        [SerializeField] private ManaCostTypeController m_ManaCostTypeController = null;

        private GameObjectPool<IconHolder> m_IconPool = null;
        private List<ManaSymbol> m_ManaSymbols = new List<ManaSymbol>();
        private List<IconHolder> m_IconHolders = new List<IconHolder>();

        private void Awake()
        {
            m_IconPool = new GameObjectPool<IconHolder>(manaCostHolderPrefab, m_IconHolder, 10,null);
            m_ManaToSpriteLibrary.InitializeDictionary();
        }

        private ManaSymbol TextToSymbol(string symbol)
        {
            switch (symbol)
            {
                case "1":
                    return ManaSymbol.Colorless;
                case "W":
                    return ManaSymbol.White;
                case "B":
                    return ManaSymbol.Black;
                case "U":
                    return ManaSymbol.Blue;
                case "G":
                    return ManaSymbol.Green;
                case "R":
                    return ManaSymbol.Red;
            }

            return ManaSymbol.Colorless;
        }

        private Sprite ManaSymbolToSprite(ManaSymbol symbol)
        {
            return m_ManaToSpriteLibrary.GetViaKey(symbol);
        }
        
        public void AddSymbol(string symbol)
        {
            ManaSymbol manaSymbol = TextToSymbol(symbol);
            AddSymbolIconHolder(manaSymbol);
        }

        private void AddSymbolIconHolder(ManaSymbol manaSymbol)
        {
            IconHolder manaCostHolder = m_IconPool.Take();
            manaCostHolder.Initialize(ManaSymbolToSprite(manaSymbol),manaSymbol);
            m_IconHolders.Add(manaCostHolder);
            m_ManaSymbols.Add(manaSymbol);
        }

        public void RemoveSymbol(string symbol)
        {
            ManaSymbol manaSymbol = TextToSymbol(symbol);
            RemoveSymbolIconHolder(manaSymbol);
        }
        
        private void RemoveSymbolIconHolder(ManaSymbol manaSymbol)
        {
            for (int i = 0; i < m_IconHolders.Count; i++)
            {
                if (m_IconHolders[i].ManaSymbol == manaSymbol)
                {
                    m_IconPool.Return(m_IconHolders[i]);
                    m_IconHolders.Remove(m_IconHolders[i]);
                    m_ManaSymbols.Remove(manaSymbol);
                    return;
                }
            }
            
            Debug.LogError("No Symbol found");
        }

        private string ComputeRequest()
        {
            int colorlessCount = 0;
            string request = "";

            for (int i = 0; i < m_ManaSymbols.Count; i++)
            {
                switch (m_ManaSymbols[i])
                {
                    case ManaSymbol.Red:
                        request += "R";
                        break;
                    case ManaSymbol.Green:
                        request += "G";
                        break;
                    case ManaSymbol.Blue:
                        request += "U";
                        break;
                    case ManaSymbol.White:
                        request += "W";
                        break;
                    case ManaSymbol.Black:
                        request += "B";
                        break;
                    case ManaSymbol.Colorless:
                        colorlessCount += 1;
                        break;
                }
            }

            if (colorlessCount > 0)
                request += colorlessCount;

            return request;
        }
        
        public string GetFilter()
        {
            if (ComputeRequest() == "")
                return "";
            
            return "+m" + ComputeSearchType() +  ComputeRequest();
        }

        private string ComputeSearchType()
        {
            switch (m_ManaCostTypeController.SearchType)
            {
                case ManaSymbolSearchType.Same:
                    return "=";
                case ManaSymbolSearchType.Contains:
                    return ":";
            }

            return ":";
        }
    }
}