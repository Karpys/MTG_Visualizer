namespace Script
{
    using System;
    using TMPro;
    using UnityEngine;

    public class CardDescriptionFilter : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_InputField = null;

        private string m_OracleTextPrefix = "o:\"";
        private string m_OracleExcludeTextPrefix = "-o:\"";

        public string GetFilter()
        {
            if (m_InputField.text == "")
                return "";
            
            string[] splitText = m_InputField.text.Split(' ');

            string completeText = "+";

            foreach (string text in splitText)
            {
                string prefix = ChoosePrefix(text,out string requestText);
                completeText += prefix + Uri.EscapeDataString(requestText) + "\" ";
            }

            return completeText;
        }

        private string ChoosePrefix(string text,out string requestText)
        {
            requestText = text;
            if (text.Length <= 0)
                return m_OracleTextPrefix;

            if (text[0] == '!')
            {
                requestText = text.Remove(0, 1);
                return m_OracleExcludeTextPrefix;
            }
            
            return m_OracleTextPrefix;
        }
    }
}