using System.Collections;
using System.Collections.Generic;
using System.IO;
using MTG;
using UnityEngine;

public class SocketInterpretor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private HolderManager m_Holder = null;
    private string[] lines;
    void Start()
    {
        UnityEngine.Application.runInBackground = true;
    }

    // Update is called once per frame
    void Update()
    {
        string path = "Assets/SocketToUnity.txt";
        lines = File.ReadAllLines(path);
        for (int i = 0; i < lines.Length; i++)
        {
            Interpret(lines[i]);
        }
        
        File.WriteAllText(path,string.Empty);
    }

    private void Interpret(string line)
    {
        if (line == "draw")
        {
            m_Holder.Draw();
            Debug.Log("Draw a card");
            return;
        }

        string[] split = line.Split(' ');

        CardState from;
        from = GetState(split[0]);
        
        CardState to;
        to = GetState(split[2]);

        m_Holder.GotoCard(to,m_Holder.GetCard(from,int.Parse(split[1])));
    }

    private CardState GetState(string state)
    {
        CardState cardState;
        CardState.TryParse(state, out cardState);
        return cardState;
    }
}
