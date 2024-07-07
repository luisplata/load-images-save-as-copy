using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace V2.Steps
{
    public class Step_4_SaveImageCreatedAndSelected : Step
    {
        [SerializeField] private List<ImageLoadingFromUrl> imageLoadingFromUrls;
        private int imageSelected;
        private TeaTime flow;
        private bool readyToNextStep;

        private byte[] imageBytes;

        public static bool IsValidUrl(string url)
        {
            Debug.Log($"Validating URL: {url}");
            bool result = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
        
        public override void StartStep()
        {
            //Quiero recorrer `stepsConfig.GetImages()` y saber si hay alguna URL en la lista y si no hay ninguna. QUiero lanzar un error
            
            
            var imagesWithoutUrl = stepsConfig.GetImages().FindAll(IsValidUrl);
            
            if (imagesWithoutUrl.Count == 0)
            {
                stepsConfig.ErrorHandling.ShowError("Error, no hay imagen para guardar");
                return;
            }
            
            for (var i = 0; i < imageLoadingFromUrls.Count; i++)
            {
                if (IsValidUrl(stepsConfig.GetImages()[i]))
                {
                    imageLoadingFromUrls[i].LoadImage(stepsConfig.GetImages()[i]);
                    imageLoadingFromUrls[i].OnFinishLoading += () => { };
                    imageLoadingFromUrls[i].OnSelectImage += bytes =>
                    {
                        imageBytes = bytes;
                        readyToNextStep = true;
                    };
                }
                else
                {
                    imageLoadingFromUrls[i].ErrorToLoadImage();
                }
            }

            flow = this.tt().Pause()
                .Add(() => { }).Wait(() => readyToNextStep).Add(() =>
                {
                    readyToNextStep = false;
                    ShowNextButton();
                });

            flow.Play();
        }

        protected override void NextAction()
        {
            SaveImageInDisk(imageBytes);
        }

        private void SaveImageInDisk(byte[] imageBytes)
        {
            try
            {
                if (imageBytes != null)
                {
                    string pathToSave = Path.Combine(Application.persistentDataPath, "Pictures/Photoleap");

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    string pathToSaveT = Path.Combine(Application.temporaryCachePath, "PhotoLeap");
                    if (!Directory.Exists(pathToSaveT))
                    {
                        Directory.CreateDirectory(pathToSaveT);
                    }

                    var nombreArchivo = $"Photoleap_imagen_modificada_{DateTime.Now:yyyyMMddHHmmss}.jpg";
                    var filePath = Path.Combine(pathToSaveT, nombreArchivo);
                    File.WriteAllBytes(filePath, imageBytes);
                    Debug.Log("Imagen modificada guardada en: " + filePath);
                    NativeFilePicker.ExportFile(filePath, success =>
                    {
                        Debug.Log($"Archivo exportado: {success}");
                        if (success)
                        {
                            NextStep.StartStep();
                            stepsConfig.NextStep();
                            File.Delete(filePath);
                        }
                        else
                        {
                            stepsConfig.ErrorHandling.ShowError("Error, no hay imagen para guardar");
                        }
                    });
                }
                else
                {
                    stepsConfig.ErrorHandling.ShowError("Error, no hay imagen para guardar");
                }
            }
            catch (Exception e)
            {
                stepsConfig.ErrorHandling.ShowError($"Error, no hay imagen para guardar {e.Message}");
            }
        }
    }
}