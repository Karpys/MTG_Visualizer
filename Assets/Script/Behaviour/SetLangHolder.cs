using System;
using UnityEngine;

namespace Script
{
    public class SetLangHolder : MonoBehaviour
    {
        [SerializeField] private Transform m_Outline = null;
        [SerializeField] private string m_Lang = String.Empty;
        public string Lang => m_Lang;
        public void Enable(bool enable)
        {
            m_Outline.gameObject.SetActive(enable);
        }
    }
}