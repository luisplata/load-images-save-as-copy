using UnityEngine;
using UnityEngine.Events;

public class Initializer : MonoBehaviour
{
    [SerializeField] private string endpoint;
    [SerializeField] private HttpRequestMediator httpRequestMediator;
    
    public UnityEvent OnStartApp;
    public UnityEvent OnFinishApp;
    public UnityEvent OnErrorApp;

    private void Start()
    {
        httpRequestMediator.SendRequest(() =>
        {
            Debug.Log("App iniciada");
            OnStartApp?.Invoke();
        }, () =>
        {
            Debug.Log("Error al iniciar la app");
            OnErrorApp?.Invoke();
        });
    }
}