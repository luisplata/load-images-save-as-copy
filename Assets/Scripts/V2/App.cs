using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace V2
{
    public class App : MonoBehaviour
    {
        public UnityEvent OnStartApp;
        [SerializeField] private ErrorHandling errorHandling;
        private void Start()
        {
            if (SaveAndLoadData.LoadData("token") == null)
            {
                Debug.Log("Token no encontrado");
                SceneManager.LoadScene(0);
                return;
            }
            OnStartApp?.Invoke();
        }
        
        public ErrorHandling ErrorHandling => errorHandling;
    }
}