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
        public override void StartStep()
        {
            for (var i = 0; i < imageLoadingFromUrls.Count; i++)
            {
                imageLoadingFromUrls[i].LoadImage(stepsConfig.GetImages()[i]);
                imageLoadingFromUrls[i].OnFinishLoading += () =>
                {
                    imageSelected++;
                    if (imageSelected == imageLoadingFromUrls.Count)
                    {
                        readyToNextStep = true;
                    }
                };
                imageLoadingFromUrls[i].OnSelectImage += bytes =>
                {
                    imageBytes = bytes;
                    readyToNextStep = true;
                };
            }
            
            flow = this.tt().Pause()
                .Add(() =>
                {
                    foreach (var imageLoadingFromUrl in imageLoadingFromUrls)
                    {
                        //imageLoadingFromUrl.Dont
                    }
                }).Wait(() => readyToNextStep).Add(() =>
                {
                    readyToNextStep = false;
                });
            
            flow.Play();
            
        }

        protected override void NextAction()
        {
            SaveImageInDisk(imageBytes);
        }


        private void SaveImageInDisk(byte[] imageBytes)
        {
            if (imageBytes != null)
            {
                string pathToSave = Application.persistentDataPath + "/Pictures/PhotoLeap/";
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var nombreArchivo = $"Photoleap_imagen_modificada_{DateTime.Now:yyyyMMddHHmmss}.jpg";
                var filePath = Path.Combine(pathToSave, nombreArchivo);

                File.WriteAllBytes(filePath, imageBytes);
                Debug.Log("Imagen modificada guardada en: " + filePath);

                var permission =
                    NativeFilePicker.ExportFile(filePath, (success) => Debug.Log($"Archivo exportado: {success}"));
                Debug.Log("Resultado del permiso: " + permission);
            }
        }
    }
}