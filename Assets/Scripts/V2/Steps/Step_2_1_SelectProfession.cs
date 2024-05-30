using TMPro;
using UnityEngine;

namespace V2.Steps
{
    public class Step_2_1_SelectProfession : Step
    {
        [SerializeField] private TMP_Dropdown professionDropdown;
        private TeaTime flow;
        private bool readyToNextStep;

        public override void StartStep()
        {
            flow = this.tt().Pause()
                .Add(() =>
                {
                    professionDropdown.onValueChanged.AddListener(value =>
                    {
                        stepsConfig.SaveProfessionSelected(professionDropdown.options[value].text);
                        readyToNextStep = true;
                    });
                }).Wait(() => readyToNextStep).Add(ShowNextButton);
            flow.Play();
        }
    }
}