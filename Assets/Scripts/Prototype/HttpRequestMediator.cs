using System;
using System.Collections.Generic;
using UnityEngine;
using V2;

public class HttpRequestMediator : MonoBehaviour, IHttpRequest
{
    [SerializeField] private string endpoint;
    [SerializeField] private GetTokenOfSession getTokenOfSession;
    [SerializeField] private Imagine imagine;

    public void CanInit(Action OnStartApp, Action OnErrorApp)
    {
        getTokenOfSession.PostRequest(endpoint, (token) =>
        {
            SaveAndLoadData.SaveData("token", token);
            OnStartApp?.Invoke();
        }, () =>
        {
            Debug.Log("Error al obtener el token");
            OnErrorApp?.Invoke();
        });
    }

    public void ImagineRequest(byte[] imageInBytes, string style, string profession, Action<List<string>> ok,
        Action error)
    {
        try
        {
            imagine.ImagineRequest(endpoint, Convert.ToBase64String(imageInBytes), style, profession, (d) =>
            {
                Debug.Log("Imagen obtenida");
                ok?.Invoke(d.upscale);
            }, (e) =>
            {
                Debug.Log($"Error al obtener la imagen {e}");
                error?.Invoke();
            });
        }
        catch (Exception e)
        {
            Debug.Log($"Error: {e}");
            error?.Invoke();
        }
    }
}