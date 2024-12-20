namespace Script
{
    using System;
    using TMPro;
    using UnityEngine;

    public class MinMaxCostFilter : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_MinValue = null;
        [SerializeField] private TMP_InputField m_MaxValue = null;
        [SerializeField] private string m_Filter = String.Empty;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                GetFilter();
            }
        }

        public string GetFilter()
        {
            if (m_MinValue.text != "" && m_MaxValue.text == "")
            {
                return String.Concat("+", m_Filter, ">", (int.Parse(m_MinValue.text) - 1));
            }
            
            if (m_MaxValue.text != "" && m_MinValue.text == "")
            {
                return String.Concat("+", m_Filter, "<", (int.Parse(m_MaxValue.text) - 1));
            }
            
            if (m_MinValue.text != "" && m_MaxValue.text != "")
            {
                return String.Concat("+", m_Filter, "=",int.Parse(m_MinValue.text),"..",int.Parse(m_MaxValue.text));
            }

            return "";
        }
    }
}