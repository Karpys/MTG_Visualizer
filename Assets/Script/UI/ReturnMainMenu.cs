using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class ReturnMainMenu : MonoBehaviour
    {
        private void Awake()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(GlobalCanvas.Instance.ReturnMainMenu);
        }

        private void OnDestroy()
        {
            Button button = GetComponent<Button>();
            button.onClick.RemoveListener(GlobalCanvas.Instance.ReturnMainMenu);
        }
    }
}