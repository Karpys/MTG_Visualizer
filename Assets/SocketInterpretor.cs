using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using MTG;
using UnityEngine;

public class SocketInterpretor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private HolderManager m_Holder = null;

    private Thread m_InterpretThread = null;
    private bool requestClear = false;

    private List<string> lines = new List<string>();
    private static object _lock = new object();
    void Start()
    {
        Application.runInBackground = true;
        m_InterpretThread = new Thread(ReadLines);
        m_InterpretThread.Start();
    }

    private void OnDestroy()
    {
        m_InterpretThread.Abort();
    }

    private void Update()
    {
        if (lines.Count > 0)
        {
            Debug.Log(lines.Count + "action count");
            for (int i = 0; i < lines.Count; i++)
            {
                Interpret(lines[i]);
            }
            lines.Clear();
            Debug.Log("Request Clear");
            requestClear = true;
        }
    }

    private void ReadLines()
    {
        while (true)
        {
            string path = "Assets/Socket/SocketToUnity.txt";

            if (requestClear)
            {
                lock (_lock)
                {
                    Debug.Log("Request Clear action");
                    File.WriteAllText(path,string.Empty);
                    requestClear = false;
                    continue;
                }
            }

            lock (_lock)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fs))
                {
                    lines.Clear();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }
            /*lines = File.ReadAllLines(path).ToList();*/
        }
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
