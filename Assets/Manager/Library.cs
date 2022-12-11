using UnityEngine;

namespace MTG
{
    public class Library:SingletonMonoBehaviour<Library>
    {
        public CardHolder m_CardHolder = null;
        public DeckHolder m_DeckHolder = null;
    }
}