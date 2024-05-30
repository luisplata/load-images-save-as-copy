using System.Collections.Generic;
using UnityEngine;

namespace V2.Steps
{
    public class Step_2_0_SelectStyle : Step
    {
        [SerializeField] private List<ImagenStyleSelection> imageStyleSelection;
        private TeaTime flow;

        public bool ReadyToNextStep { get; set; }

        public StyleToImageMidjourney StyleToImageMidjourney { get; set; }

        public override void StartStep()
        {
            foreach (var styleSelection in imageStyleSelection)
            {
                styleSelection.Configure(this);
            }
            flow = this.tt().Pause()
                .Add(() => { }).Wait(() => ReadyToNextStep).Add(() =>
                {
                    stepsConfig.SaveStyleSelected(StyleToImageMidjourney);
                    ShowNextButton();
                });
            flow.Play();
            ReadyToNextStep = false;
        }
    }

    public enum StyleToImageMidjourney
    {
        Style1,
        Style2,
        Style3,
        Style4,
    }
}