namespace Script.Helper
{
    using System.IO;
    using UI;

    public static class DeckHelper
    {
        public static void SaveDeck(this DeckData deckData)
        {
            string[] deckFile = deckData.ToFile();
            File.WriteAllLines(CardFileHelper.GetDeckPath() + deckData.DeckName + ".deck",deckFile);
        }
        
        public static void CreateDeckFile(string deckName,int deckType,string deckBackFileName)
        {
            string path = CardFileHelper.GetDeckPath() + deckName + ".deck";
            string[] deckData = new string[9];
            deckData[0] = deckName;
            deckData[1] = deckBackFileName;
            deckData[2] = deckType.ToString();
            deckData[3] = "Deck";
            deckData[4] = "EndDeck";
            deckData[5] = "Token";
            deckData[6] = "EndToken";
            deckData[7] = "Commander";
            deckData[8] = "EndCommander";
            File.WriteAllLines(path,deckData);
        }

        public static string DeckNameToDeckFile(string deckName)
        {
            return CardFileHelper.GetDeckPath() + deckName + ".deck";
        }
    }
}