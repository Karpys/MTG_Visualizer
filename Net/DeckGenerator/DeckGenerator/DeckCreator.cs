namespace DeckGenerator;

public class DeckCreator
{
    public string baseScriptablePath = "../../../../../../Assets/DeckGenerator/BaseCopy.asset";
    private string deckPath = "../../../../../../Assets/Deck/";
    private string deckName = Console.ReadLine();
    private string spritesPath;
    private string scriptablePath;
    public void GenerateDeck()
    {
        spritesPath = deckPath + deckName + "/Sprite";
        scriptablePath = deckPath + deckName + "/Card";
        Directory.CreateDirectory(scriptablePath);
        
        List<string> files = Directory.GetFiles(spritesPath).ToList();
        
        KeepOnlyMeta(files);

        /*foreach (string file in files)
        {
            Console.WriteLine(file);
        }*/

        CreateDeck(files);

    }

    private void CreateDeck(List<string> files)
    {
        string[] baseCopy = GetBaseScriptable();
        string[] names = GetNames(files);
        string[] guiId = GetSpriteId(files);
        
        
        for(int i = 0; i < names.Length;i++)
        {
            string assetPath = scriptablePath + "/" + names[i] + ".asset";
            File.Create(assetPath).Close();
            for (int y = 0; y < baseCopy.Length; y++)
            {
                if (baseCopy[y].Contains("m_CardVisual"))
                {
                    string[] copy = baseCopy[y].Split(' ');
                    copy[6] = guiId[i] + ",";
                    baseCopy[y] = String.Join(' ', copy);
                }else if (baseCopy[y].Contains("m_Name"))
                {
                    baseCopy[y] = "  m_Name: " + names[i];
                }
            }
            
            File.WriteAllLines(assetPath,baseCopy);
        }
    }

    private string[] GetSpriteId(List<string> files)
    {
        string[] guiIds = new string[files.Count];

        int id = 0;
        foreach (string file in files)
        {
            string[] lines = File.ReadAllLines(file);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("guid"))
                {
                    guiIds[id] = lines[i].Split()[1];
                }
            }

            id++;
        }

        return guiIds;
    }

    private string[] GetNames(List<string> files)
    {
        string[] names = new string[files.Count];
        
        for (int i = 0; i < files.Count; i++)
        {
            names[i] = files[i].Split('\\','/', '.')[22];
        }

        return names;
    }

    private string[] GetBaseScriptable()
    {
        return File.ReadAllLines(baseScriptablePath);
    }

    private void KeepOnlyMeta(List<string> files)
    {
        for (int i = 0; i < files.Count; i++)
        {
            if (!files[i].Contains(".meta"))
            {
                files.Remove(files[i]);
                i--;
            }
        }
    }
}