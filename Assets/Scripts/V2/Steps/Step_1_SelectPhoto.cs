using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace V2.Steps
{
    public class Step_1_SelectPhoto : Step
    {
        [SerializeField] private Button selectPhotoButton;
        [SerializeField] private Image image;
        private bool readyToNextStep, _fileSelected;
        private string path;

        private TeaTime flow;

        private void ChooseFile()
        {
            flow.Play();
        }

        private IEnumerator LoadImageFromDisk(string pathToLoad)
        {
            yield return null;
            if (File.Exists(pathToLoad))
            {
                byte[] imageBytes = File.ReadAllBytes(pathToLoad);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imageBytes);
                stepsConfig.SetImageBytes(imageBytes);
                readyToNextStep = true;
                image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                yield return tex;
            }
            else
            {
                Debug.Log("File does not exist.");
                yield return null;
            }
        }

        public override void StartStep()
        {
            selectPhotoButton.onClick.AddListener(ChooseFile);
            flow = this.tt().Pause()
                .Add(() =>
                {
#if UNITY_ANDROID
                    string[] fileTypes = { "image/*" };
#elif !UNITY_ANDROID
            string[] fileTypes = new string[] { "public.image"};
#endif
                    NativeFilePicker.PickFile(_path =>
                    {
                        _fileSelected = _path != null;
                        if (_fileSelected)
                        {
                            path = _path;
                        }
                        else
                        {
                            Debug.Log("Operación cancelada");
                        }

                        readyToNextStep = true;
                    }, fileTypes);
                }).Wait(() => readyToNextStep)
                .Add(() =>
                {
                    if (_fileSelected)
                    {
                        StartCoroutine(LoadImageFromDisk(path));
                    }
                }).Wait(() => readyToNextStep).Add(() =>
                {
                    if (_fileSelected)
                    {
                        ShowNextButton();
                    }

                    readyToNextStep = false;
                });
        }
    }
}