using UnityEngine;
using UnityEngine.UI;

namespace V2.Steps
{
    internal class ImagenStyleSelection : MonoBehaviour
    {
        [SerializeField] private Button selectStyleButton;
        [SerializeField] private StyleToImageMidjourney style;
        public void Configure(Step_2_0_SelectStyle step20SelectStyle)
        {
            selectStyleButton.onClick.AddListener(() =>
            {
                Debug.Log("Style selected");
                step20SelectStyle.ReadyToNextStep = true;
                step20SelectStyle.StyleToImageMidjourney = style;
            });
        }
    }
}