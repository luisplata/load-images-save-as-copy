using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace V2
{
    public class ErrorHandling : MonoBehaviour
    {
        [SerializeField] private GameObject errorPanel;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private Button closeButton;

        private void Start()
        {
            closeButton.onClick.AddListener(() =>
            {
                RestartApp();
                HideError();
            });
        }

        public void ShowError(string message)
        {
            errorText.text = message;
            errorPanel.SetActive(true);
        }
        
        public void HideError()
        {
            errorPanel.SetActive(false);
        }
        
        public void RestartApp()
        {
            SceneManager.LoadScene(0);
        }
    }
}