namespace Script
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ManaCostFilter : MonoBehaviour
    {
        private string m_Request = String.Empty;
        private ManaSymbolSearchType m_ManaSearchType = ManaSymbolSearchType.Contains;
        private List<ManaSymbol> m_ManaSymbols = new List<ManaSymbol>();
        
        public void AddSymbol(string symbol)
        {
            switch (symbol)
            {
                case "1":
                    m_ManaSymbols.Add(ManaSymbol.Colorless);
                    break;
                case "W":
                    m_ManaSymbols.Add(ManaSymbol.White);
                    break;
                case "B":
                    m_ManaSymbols.Add(ManaSymbol.Black);
                    break;
                case "U":
                    m_ManaSymbols.Add(ManaSymbol.Blue);
                    break;
                case "G":
                    m_ManaSymbols.Add(ManaSymbol.Green);
                    break;
                case "R":
                    m_ManaSymbols.Add(ManaSymbol.Red);
                    break;
            }

            UpdateVisual();
        }
        
        public void RemoveSymbol(string symbol)
        {
            switch (symbol)
            {
                case "1":
                    m_ManaSymbols.Remove(ManaSymbol.Colorless);
                    break;
                case "W":
                    m_ManaSymbols.Remove(ManaSymbol.White);
                    break;
                case "B":
                    m_ManaSymbols.Remove(ManaSymbol.Black);
                    break;
                case "U":
                    m_ManaSymbols.Remove(ManaSymbol.Blue);
                    break;
                case "G":
                    m_ManaSymbols.Remove(ManaSymbol.Green);
                    break;
                case "R":
                    m_ManaSymbols.Remove(ManaSymbol.Red);
                    break;
            }
            
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            
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
            switch (m_ManaSearchType)
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