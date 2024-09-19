using System;
using System.Collections.Generic;
using Plugins.ServiceLocator;
using UnityEngine;
using V2;

public class HttpRequestMediator : MonoBehaviour, IHttpRequest
{
    [SerializeField] private GetTokenOfSession getTokenOfSession;
    [SerializeField] private Imagine imagine;

    public void CanInit(Action OnStartApp, Action<string> OnErrorApp)
    {
        getTokenOfSession.PostRequest(ServiceLocator.Instance.GetService<ILoadData>().LoadData("endpoint"), (token) =>
        {
            ServiceLocator.Instance.GetService<ISaveData>().SaveData("token", token);
            OnStartApp?.Invoke();
        }, e =>
        {
            Debug.Log("Error al obtener el token");
            OnErrorApp?.Invoke(e);
        });
    }

    public void ImagineRequest(byte[] imageInBytes, string style, string profession, Action<List<string>> ok,
        Action error)
    {
        try
        {
            imagine.ImagineRequest(ServiceLocator.Instance.GetService<ILoadData>().LoadData("endpoint"), Convert.ToBase64String(imageInBytes), style, profession, (d) =>
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