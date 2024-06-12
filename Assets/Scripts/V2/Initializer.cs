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
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(sceneIndex);
        }
    }
}