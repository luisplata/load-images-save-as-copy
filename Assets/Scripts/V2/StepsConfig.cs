using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using V2.Steps;

namespace V2
{
    public class StepsConfig : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private int countOfSteps;
        private int currentStep;
        private byte[] imageBytes;
        private List<string> _upScales;
        private StyleToImageMidjourney _styleSaved;
        private string _professionSaved;

        private void Start()
        {
            scrollRect.onValueChanged.AddListener((v) => { Debug.Log($"Scroll value: {v}"); });
            SetStep(currentStep);
        }

        private void SetStep(int step)
        {
            Debug.Log($"StepsConfig SetStep {step} / {countOfSteps}");
            if (step < 0 || step > countOfSteps)
            {
                Debug.LogError("Step out of range");
                return;
            }

            scrollRect.horizontalNormalizedPosition = (float)step / countOfSteps;
        }

        public void NextStep()
        {
            Debug.Log("StepsConfig NextStep");
            currentStep++;
            SetStep(currentStep);
        }

        public void SetImageBytes(byte[] readAllBytes)
        {
            imageBytes = readAllBytes;
        }

        public byte[] GetImageBytes()
        {
            return imageBytes;
        }

        public void SaveImages(List<string> upScales)
        {
            _upScales = upScales;
        }

        public List<string> GetImages()
        {
            return _upScales;
        }

        public void SaveStyleSelected(StyleToImageMidjourney bytes)
        {
            _styleSaved = bytes;
        }

        public string GetStyle()
        {
            return _styleSaved.ToString();
        }

        public string GetProfession()
        {
            return _professionSaved;
        }

        public void SaveProfessionSelected(string text)
        {
            _professionSaved = text;
        }
    }
}