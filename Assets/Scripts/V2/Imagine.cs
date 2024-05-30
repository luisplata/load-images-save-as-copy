using System;
using System.Collections.Generic;
using UnityEngine;

namespace V2
{
    public class Imagine : MonoBehaviour
    {
        [SerializeField] private string uri;

        public void ImagineRequest(string baseUrl, string imageInBase64, string style, string profession, Action<ImagineResponse> actionOk, Action<string> actionError)
        {
            var request = new HttpPostRequest<ImagineResponse>();
            //Need serializable image to base64
            
            Debug.Log($"Image: {imageInBase64}");
            Debug.Log($"Style: {style}");
            Debug.Log($"Profession: {profession}");
            Debug.Log($"Url: {baseUrl + uri}"); 

            var image = new ImageSerializable
            {
                image = imageInBase64,
                style = style,
                context = profession
                //style = "a steampunk illustration of a doctor in a Victorian consultation room, inspired by H.G. Wells, detailed scene with the doctor using antique equipment",
                //context = "warm color temperature, dim lighting, and mysterious atmosphere";
            };
            Debug.Log($"Image: {image}");
            StartCoroutine(request.SendRequestWithToken(baseUrl + uri, image,
                d =>
                {
                    Debug.Log($"Imagen enviada");
                    actionOk?.Invoke(d);
                }, error =>
                {
                    Debug.Log($"Error al enviar la imagen {error}");
                    actionError?.Invoke(error);
                }));
        }
    }

    [Serializable]
    public class ImagineResponse
    {
        public string message;
        public string result;
        public List<string> upscale;
        public string prompt;
    }

    [Serializable]
    public class ImageSerializable
    {
        public string image;
        public string style;
        public string context;
    }
}