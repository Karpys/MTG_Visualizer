namespace Script
{
    using TMPro;
    using UnityEngine;

    public class CardManaCostFilter : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_MinValue = null;
        [SerializeField] private TMP_InputField m_MaxValue = null;

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
                return "+cmc>" + (int.Parse(m_MinValue.text) - 1);
            }
            
            if (m_MaxValue.text != "" && m_MinValue.text == "")
            {
                return "+cmc<" +(int.Parse(m_MaxValue.text) - 1);
            }
            
            if (m_MinValue.text != "" && m_MaxValue.text != "")
            {
                return "+cmc=" + int.Parse(m_MinValue.text) + ".." + int.Parse(m_MaxValue.text);
            }

            return "";
        }
    }
}