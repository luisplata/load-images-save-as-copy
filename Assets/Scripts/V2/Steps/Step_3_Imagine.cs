using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace V2.Steps
{
    public class Step_3_Imagine : Step
    {
        [SerializeField] private Image icon;
        [SerializeField, InterfaceType(typeof(IHttpRequest))]
        private Object http;
        private IHttpRequest Http => http as IHttpRequest;
        
        private bool canRotate = true;

        private void Update()
        {
            if (canRotate)
            {
                //rotar el icono
                icon.transform.Rotate(Vector3.forward, 1);
            }
        }

        public override void StartStep()
        {
            TryImagineRequest(0); // Inicia el proceso con 0 reintentos
        }
        
        private void TryImagineRequest(int retryCount)
        {
            const int maxRetries = 5;
            Debug.Log("LoadImageAndCreateNewImage");
            Http.ImagineRequest(stepsConfig.GetImageBytes(), stepsConfig.GetStyle(), stepsConfig.GetProfession(), (response) =>
            {
                stepsConfig.SaveImages(response);
                NextStep.StartStep();
                ShowNextButton();
                canRotate = false;
                //reubicar con la rotacion en 0
                icon.transform.rotation = Quaternion.identity;
            }, () =>
            {
                if (retryCount < maxRetries)
                {
                    Debug.Log($"Error al cargar la imagen. Reintentando... Intento {retryCount + 1}");
                    TryImagineRequest(retryCount + 1); // Reintenta la solicitud
                    canRotate = true;
                }
                else
                {
                    stepsConfig.ErrorHandling.ShowError("Error al cargar la imagen después de varios intentos.");
                }
            });
        }

        protected override void NextAction()
        {
            //base.NextAction();
            stepsConfig.NextStep();
        }
    }
}