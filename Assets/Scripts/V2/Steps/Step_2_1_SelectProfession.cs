using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace V2.Steps
{
    public class Step_2_1_SelectProfession : Step
    {
        [SerializeField] private TMP_Dropdown professionDropdown;
        private TeaTime flow;
        private bool readyToNextStep;

        private Dictionary<string, string> professions = new()
        {
            { "Grupo 1", "Grupo1" },
            { "Grupo 2", "Grupo2" },
            { "Grupo 3", "Grupo3" },
            { "Grupo 4", "Grupo4" },
            { "Grupo 5", "Grupo5" },
            { "Grupo 6", "Grupo6" },
            { "Grupo 7", "Grupo7" },
            { "Grupo 8", "Grupo8" },
            { "Grupo 9", "Grupo9" },
            { "Grupo 10", "Grupo10" },
            { "Grupo 11", "Grupo11" },
            { "Grupo 12", "Grupo12" },
            { "Grupo 13", "Grupo13" },
            { "Grupo 14", "Grupo14" }
        };

        protected override void Start()
        {
            base.Start();

            professionDropdown.onValueChanged.AddListener(value =>
            {
                if (value == 0)
                {
                    HideNextButton();
                    readyToNextStep = false;
                    flow.Play();
                    return;
                }

                stepsConfig.SaveProfessionSelected(professions[professionDropdown.options[value].text]);
                readyToNextStep = true;
            });
        }

        public override void StartStep()
        {
            flow = this.tt().Pause()
                .Add(() => { }).Wait(() => readyToNextStep).Add(ShowNextButton);
            flow.Play();
        }
    }
}