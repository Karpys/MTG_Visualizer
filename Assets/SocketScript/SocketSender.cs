using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using MTG;
using UnityEngine;
using ThreadPriority = UnityEngine.ThreadPriority;

public class SocketSender : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Holder m_Hand = null;

    private Thread m_WriteThread = null;
    private bool m_RequestWrite = false;
    private string[] m_LinesToWrite = new string[0];
    private void Start()
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        m_WriteThread = new Thread(WriteLines);
        m_WriteThread.Start();
    }

    private void OnDestroy()
    {
        m_WriteThread.Abort();
    }

    public void RequestWrite()
    {
        string cards = String.Empty;

        for (int i = 0; i < m_Hand.Cards.Count; i++)
        {
                
            cards +=" "+m_Hand.Cards[i].gameObject.name ;
        }
        
        m_LinesToWrite = new string[3];
        m_LinesToWrite[0] = cards;
        m_RequestWrite = true;
    }

    private void WriteLines()
    {
        while (true)
        {
            if(!m_RequestWrite) continue;
            Debug.Log("Write lines");
            string path = "Assets/Socket/SocketToHand.txt";
            File.WriteAllLines(path, m_LinesToWrite);
            m_RequestWrite = false;
        }
    }
}
