using Script.UI;

namespace Script.Manager
{
    public static class DeckDataHolder
    {
        public static DeckData DeckData;

        public static void SetDeckData(DeckData deckData)
        {
            DeckData = deckData;
        }
    }
}