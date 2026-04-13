namespace Script.UI
{
    using System;
    using UnityEngine;

    public class ManaCostHolder : MonoBehaviour
    {
        [SerializeField] private string m_Symbol = "";
        [SerializeField] private ManaCostFilter m_ManaCostFilter = null;

        private int m_CurrentCount = 0;

        public void OnValueChanged(string value)
        {
            if (int.TryParse(value, out int intValue))
            {
                if(m_CurrentCount == intValue || intValue < 0)
                    return;

                int diff = intValue - m_CurrentCount;

                if (diff > 0)
                    Add(diff);
                else
                {
                    Remove(Math.Abs(diff));
                }

                m_CurrentCount = intValue;
            }
            else
            {
                RemoveAllSymbol();
            }
        }

        private void RemoveAllSymbol()
        {
            for (int i = 0; i < m_CurrentCount; i++)
            {
                m_ManaCostFilter.RemoveSymbol(m_Symbol);
            }

            m_CurrentCount = 0;
        }

        private void Add(int count)
        {
            for (int i = 0; i < count; i++)
            {
                m_ManaCostFilter.AddSymbol(m_Symbol);
            }
        }

        private void Remove(int count)
        {
            for (int i = 0; i < count; i++)
            {
                m_ManaCostFilter.RemoveSymbol(m_Symbol);
            }
        }
    }
}