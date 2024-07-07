using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace V2
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private string endpoint;
        [SerializeField, InterfaceType(typeof(IHttpRequest))]
        private Object http;
        private IHttpRequest httpRequestMediator => http as IHttpRequest;
    
        public UnityEvent OnStartApp;
        public UnityEvent OnErrorApp;
        private SaveAndLoadData _data;

        private void Start()
        {
            _data = new SaveAndLoadData();
            httpRequestMediator.CanInit(() =>
            {
                OnStartApp?.Invoke();
            }, () =>
            {
                OnErrorApp?.Invoke();
            });
        }
    
        public void LoadScene(int sceneIndex)
        {
            StartCoroutine(LoadSceneAsync(sceneIndex));
        }

        private IEnumerator LoadSceneAsync(int sceneIndex)
        {
            int maxRetries = 3; // Número máximo de intentos
            int attempts = 0; // Contador de intentos
            bool sceneLoaded = false; // Indicador de si la escena se ha cargado

            while (attempts < maxRetries && !sceneLoaded)
            {
                yield return new WaitForSeconds(1); // Espera antes de intentar cargar la escena, para simular un reintento

                try
                {
                    SceneManager.LoadScene(sceneIndex);
                    sceneLoaded = true; // Si se carga la escena, establecer el indicador a true
                }
                catch
                {
                    // En caso de error, incrementar el contador de intentos
                    attempts++;
                }
            }

            if (!sceneLoaded)
            {
                // Si después de todos los intentos la escena no se ha cargado, cerrar la aplicación
                Application.Quit();
            }
        }
    }
}