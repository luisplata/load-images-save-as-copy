using UnityEngine;

namespace V2.Steps
{
    public class Step_3_Imagine : Step
    {
        [SerializeField, InterfaceType(typeof(IHttpRequest))]
        private Object http;
        private IHttpRequest Http => http as IHttpRequest;
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
            }, () =>
            {
                if (retryCount < maxRetries)
                {
                    Debug.Log($"Error al cargar la imagen. Reintentando... Intento {retryCount + 1}");
                    TryImagineRequest(retryCount + 1); // Reintenta la solicitud
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