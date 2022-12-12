using System.Collections.Generic;
using UnityEngine;

namespace MTG
{
    [CreateAssetMenu(fileName = "New Deck", menuName = "ScriptableObjects/Deck", order = 1)]
    public class DeckScriptable : ScriptableObject
    {
        public List<CardScriptable> m_Cards;
        public List<CardScriptable> m_Jetons;
        public Sprite m_CardBackSprite = null;
    }

}