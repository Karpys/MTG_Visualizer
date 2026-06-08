using System.Collections;
using System.Collections.Generic;
using KarpysDev.KarpysUtils.TweenCustom;
using TMPro;
using UnityEngine;

public struct NotifLogData
{
    private string _message;
    private float _duration;
    
    public float Duration => _duration;
    public string Message => _message;

    public NotifLogData(string message, float duration)
    {
        _message = message;
        _duration = duration;
    }
}
public class NotifLogHolder : MonoBehaviour
{
    [SerializeField] private Transform m_NotifLogHolder = null;
    [SerializeField] private TMP_Text m_NotifLogText = null;
    [SerializeField] private Transform m_Start = null;
    [SerializeField] private Transform m_End = null;

    private bool m_IsReady = true;
    private Queue<NotifLogData> m_MessageQueue = new Queue<NotifLogData>();

    public bool IsReady => m_IsReady;
    public void Update()
    {
        if (IsReady && m_MessageQueue.Count > 0)
        {
            LaunchLog();
        }
    }

    public void AddLog(string message, float duration = 0.25f)
    {
        m_MessageQueue.Enqueue(new NotifLogData(message, duration));
    }

    private void LaunchLog()
    {
        m_IsReady = false;
        m_NotifLogHolder.transform.position = m_Start.position;
        NotifLogData notifLog = m_MessageQueue.Dequeue();
        m_NotifLogHolder.DoMove(m_End.position, 0.15f).OnComplete(() => ResetReady(notifLog.Duration));
        m_NotifLogText.text = notifLog.Message;
    }

    private void ResetReady(float duration)
    {
        StartCoroutine(ResetState(duration));
    }

    private IEnumerator ResetState(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_NotifLogHolder.transform.position = m_Start.position;
        m_IsReady = true;
    }
}