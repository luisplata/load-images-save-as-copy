using System;
using UnityEngine;

public class HttpRequestMediator : MonoBehaviour
{
    [SerializeField] private string endpoint;
    [SerializeField] private GetTokenOfSession getTokenOfSession;
    
    public void SendRequest(Action OnStartApp, Action OnErrorApp)
    {
        getTokenOfSession.PostRequest(endpoint,(token) =>
        {
            Debug.Log("Token: " + token);
            PlayerPrefs.SetString("token", token);
            OnStartApp?.Invoke();
        }, () =>
        {
            Debug.Log("Error al obtener el token");
            OnErrorApp?.Invoke();
        });
    }
}