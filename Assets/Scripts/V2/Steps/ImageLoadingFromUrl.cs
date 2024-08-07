﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace V2.Steps
{
    public class ImageLoadingFromUrl : MonoBehaviour
    {
        [SerializeField] private Image image, imageBig;
        [SerializeField] private Button selectImageButton;
        public Action OnFinishLoading;
        public Action<byte[]> OnSelectImage;

        public void LoadImage(string url)
        {
            Debug.Log($"Loading image from {url}");
            StartCoroutine(LoadTexture(url));
            selectImageButton.onClick.AddListener(() =>
            {
                //Get bytes from image
                var texture = image.sprite.texture;
                var bytes = texture.EncodeToPNG();
                OnSelectImage?.Invoke(bytes);
                imageBig.sprite = image.sprite;
            });
        }

        private IEnumerator LoadTexture(string imageUrl)
        {
            using var www = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(www);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                image.sprite = sprite;
                Debug.Log("Image loaded");
                OnFinishLoading?.Invoke();
            }
        }

        public void ErrorToLoadImage()
        {
            image.color = Color.red;
        }
    }
}