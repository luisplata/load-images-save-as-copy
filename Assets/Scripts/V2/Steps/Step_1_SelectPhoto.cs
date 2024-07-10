using System;
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
        [SerializeField] private Button rotateLeftButton, rotateRightButton;
        private bool readyToNextStep, _fileSelected;
        private string path;

        private TeaTime flow;

        private void ChooseFile()
        {
            flow.Play();
        }

        Texture2D RotateTextureCounterClockwise(Texture2D originalTexture)
        {
            Texture2D rotatedTexture = new Texture2D(originalTexture.height, originalTexture.width);

            for (int i = 0; i < originalTexture.width; i++)
            {
                for (int j = 0; j < originalTexture.height; j++)
                {
                    rotatedTexture.SetPixel(originalTexture.height - 1 - j, i, originalTexture.GetPixel(i, j));
                }
            }

            rotatedTexture.Apply();
            return rotatedTexture;
        }

        private IEnumerator RotateTextureCounterClockwiseCoroutine(Texture2D originalTexture,
            Action<Texture2D> onCompleted)
        {
            Texture2D rotatedTexture = new Texture2D(originalTexture.height, originalTexture.width);

            for (int i = 0; i < originalTexture.width; i++)
            {
                for (int j = 0; j < originalTexture.height; j++)
                {
                    rotatedTexture.SetPixel(originalTexture.height - 1 - j, i, originalTexture.GetPixel(i, j));
                }

                // Permitir que la UI se actualice después de procesar cada fila.
                if (i % 10 == 0) // Ajusta este valor según sea necesario para equilibrar rendimiento y responsividad.
                {
                    yield return null;
                }
            }

            rotatedTexture.Apply();
            onCompleted?.Invoke(rotatedTexture);
        }

        private IEnumerator RotateTextureClockwiseCoroutine(Texture2D originalTexture, Action<Texture2D> onCompleted)
        {
            Texture2D rotatedTexture = new Texture2D(originalTexture.height, originalTexture.width);

            for (int i = 0; i < originalTexture.width; i++)
            {
                for (int j = 0; j < originalTexture.height; j++)
                {
                    rotatedTexture.SetPixel(j, originalTexture.width - 1 - i, originalTexture.GetPixel(i, j));
                }

                // Permitir que la UI se actualice después de procesar cada fila.
                if (i % 10 == 0) // Ajusta este valor según sea necesario para equilibrar rendimiento y responsividad.
                {
                    yield return null;
                }
            }

            rotatedTexture.Apply();
            onCompleted?.Invoke(rotatedTexture);
        }

        Texture2D RotateTextureClockwise(Texture2D originalTexture)
        {
            Texture2D rotatedTexture = new Texture2D(originalTexture.height, originalTexture.width);

            for (int i = 0; i < originalTexture.width; i++)
            {
                for (int j = 0; j < originalTexture.height; j++)
                {
                    rotatedTexture.SetPixel(j, originalTexture.width - 1 - i, originalTexture.GetPixel(i, j));
                }
            }

            rotatedTexture.Apply();
            return rotatedTexture;
        }

        public void RotateImageClockwise()
        {
            StartCoroutine(RotateImageClockwiseCoroutine());
        }

        private IEnumerator RotateImageClockwiseCoroutine()
        {
            rotateLeftButton.gameObject.SetActive(false);
            rotateRightButton.gameObject.SetActive(false);
            selectPhotoButton.gameObject.SetActive(false);
            nextStepButton.gameObject.SetActive(false);

            StartCoroutine(RotateTextureClockwiseCoroutine(image.sprite.texture, (rotatedTexture) =>
                {
                    Sprite rotatedSprite = Sprite.Create(rotatedTexture,
                        new Rect(0, 0, rotatedTexture.width, rotatedTexture.height), new Vector2(0.5f, 0.5f));
                    image.sprite = rotatedSprite;
                    rotateLeftButton.gameObject.SetActive(true);
                    rotateRightButton.gameObject.SetActive(true);
                    selectPhotoButton.gameObject.SetActive(true);
                    nextStepButton.gameObject.SetActive(true);
                })); /*
                Texture2D rotatedTexture = RotateTextureClockwise(image.sprite.texture);
                Sprite rotatedSprite = Sprite.Create(rotatedTexture,
                    new Rect(0, 0, rotatedTexture.width, rotatedTexture.height), new Vector2(0.5f, 0.5f));
                image.sprite = rotatedSprite;

                rotateLeftButton.gameObject.SetActive(true);
                rotateRightButton.gameObject.SetActive(true);*/

            yield return null;
        }

        private IEnumerator RotateImageCounterClockwiseCoroutine()
        {
            rotateLeftButton.gameObject.SetActive(false);
            rotateRightButton.gameObject.SetActive(false);
            selectPhotoButton.gameObject.SetActive(false);
            nextStepButton.gameObject.SetActive(false);

            StartCoroutine(RotateTextureCounterClockwiseCoroutine(image.sprite.texture, (rotatedTexture) =>
            {
                Sprite rotatedSprite = Sprite.Create(rotatedTexture,
                    new Rect(0, 0, rotatedTexture.width, rotatedTexture.height), new Vector2(0.5f, 0.5f));
                image.sprite = rotatedSprite;
                rotateLeftButton.gameObject.SetActive(true);
                rotateRightButton.gameObject.SetActive(true);
                selectPhotoButton.gameObject.SetActive(true);
                nextStepButton.gameObject.SetActive(true);
            }));

            /*Texture2D rotatedTexture = RotateTextureCounterClockwise(image.sprite.texture);
            Sprite rotatedSprite = Sprite.Create(rotatedTexture, new Rect(0, 0, rotatedTexture.width, rotatedTexture.height), new Vector2(0.5f, 0.5f));
            image.sprite = rotatedSprite;

            rotateLeftButton.gameObject.SetActive(true);
            rotateRightButton.gameObject.SetActive(true);*/

            yield return null;
        }

        public void RotateImageCounterClockwise()
        {
            StartCoroutine(RotateImageCounterClockwiseCoroutine());
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
            rotateLeftButton.onClick.AddListener(RotateImageCounterClockwise);
            rotateRightButton.onClick.AddListener(RotateImageClockwise);
            rotateLeftButton.gameObject.SetActive(false);
            rotateRightButton.gameObject.SetActive(false);
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
                        rotateLeftButton.gameObject.SetActive(true);
                        rotateRightButton.gameObject.SetActive(true);
                    }

                    readyToNextStep = false;
                });
        }
    }
}