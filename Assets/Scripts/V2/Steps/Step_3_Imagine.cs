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
            Debug.Log("LoadImageAndCreateNewImage");
            Http.ImagineRequest(stepsConfig.GetImageBytes(), stepsConfig.GetStyle(), stepsConfig.GetProfession(), (response) =>
            {
                stepsConfig.SaveImages(response);
                NextStep.StartStep();
                ShowNextButton();
            }, () =>
            {
                stepsConfig.ErrorHandling.ShowError("Error al cargar la imagen");
            });
        }

        protected override void NextAction()
        {
            //base.NextAction();
            stepsConfig.NextStep();
        }
    }
}