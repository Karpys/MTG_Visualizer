using UnityEngine;
using UnityEngine.UI;

namespace MTG
{
    public class UICardSelection : MonoBehaviour
    {
        [SerializeField] private int m_Index = 0;
        [SerializeField] private Image m_Image = null;
        [SerializeField] private CardState m_Holder = 0;

        public void Initialize(int index, CardState state,Sprite sprite)
        {
            m_Index = index;
            m_Holder = state;
            m_Image.sprite = sprite;
        }

        public void OnSelect()
        {
            Holder zone = HolderManager.Instance.GetHolder(m_Holder);
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                CardHelpDisplay.Instance.DisplayPreviewCard
                    (zone.Cards[m_Index]);
            }else
            {
                HolderManager.Instance.SetSelectableCard(zone.Cards[m_Index]);
            }
        }
    }
}