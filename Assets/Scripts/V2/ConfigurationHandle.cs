using Plugins.ServiceLocator;
using TMPro;
using UnityEngine;

namespace V2
{
    public class ConfigurationHandle : MonoBehaviour
    {
        [SerializeField] private GameObject configPanel;
        [SerializeField] private TMP_InputField inputField;

        public static ConfigurationHandle Instance { get; private set; }

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
            HideConfig();
            inputField.text = ServiceLocator.Instance.GetService<ILoadData>().LoadData("endpoint");
        }

        private void HideConfig()
        {
            configPanel.SetActive(false);
        }

        public void ShowConfig()
        {
            configPanel.SetActive(true);
        }
        
        
        public void SaveEndPoint()
        {
            var end = inputField.text.Trim();
            ServiceLocator.Instance.GetService<ISaveData>().SaveData("endpoint", end);
        }
    }
}