using System.Collections;
using System.Collections.Generic;
using KarpysDev.KarpysUtils.TweenCustom;
using TMPro;
using UnityEngine;

public class NotifLogHolder : MonoBehaviour
{
    [SerializeField] private Transform m_NotifLogHolder = null;
    [SerializeField] private TMP_Text m_NotifLogText = null;
    [SerializeField] private Transform m_Start = null;
    [SerializeField] private Transform m_End = null;

    private bool m_IsReady = true;
    private Queue<string> m_MessageQueue = new Queue<string>();

    public bool IsReady => m_IsReady;
    public void Update()
    {
        if (IsReady && m_MessageQueue.Count > 0)
        {
            LaunchLog();
        }
    }

    public void AddLog(string message)
    {
        m_MessageQueue.Enqueue(message);
    }

    private void LaunchLog()
    {
        m_IsReady = false;
        m_NotifLogHolder.transform.position = m_Start.position;
        m_NotifLogHolder.DoMove(m_End.position, 0.15f).OnComplete(ResetReady);
        string message = m_MessageQueue.Dequeue();
        m_NotifLogText.text = message;
    }

    private void ResetReady()
    {
        StartCoroutine(ResetState());
    }

    private IEnumerator ResetState()
    {
        yield return new WaitForSeconds(0.25f);
        m_IsReady = true;
    }
}