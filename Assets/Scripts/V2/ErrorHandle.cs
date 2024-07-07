using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace V2
{
    public class ErrorHandle : MonoBehaviour
    {
        [SerializeField] private GameObject errorPanel;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private Button closeButton;

        public static ErrorHandle Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            HideMessage();
            closeButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(0);
                HideMessage();
            });
        }

        private void HideMessage()
        {
            errorPanel.SetActive(false);
            errorText.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(false);
        }

        public void ShowError(string message)
        {
            errorPanel.SetActive(true);
            errorText.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(true);
            errorText.text = message;
            Debug.LogError(message);
        }
    }
}