using UnityEngine;
using UnityEngine.Events;

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
    
        public void LoadScene(int sceneIndex) => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}