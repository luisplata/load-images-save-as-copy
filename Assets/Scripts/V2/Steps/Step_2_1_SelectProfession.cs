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
        
        private Dictionary<string, string> professions = new Dictionary<string, string>
        {
            {"Grupo 1", "Convert the image into a stylish company CEO by incorporating business items and elegant restaurant"},
            {"Grupo 2", "Convert the image into a detailed and precise financial illustration incorporate numbers and Financial items"},
            {"Grupo 3", "Convert the image into a naturalism style incorporate agronomy trade and ecology items"},
            {"Grupo 4", "Create an artistic collage using the image and other visuals to incorporate Logistics and international Business items"},
            {"Grupo 5", "Convert image into professional Workplace Safety and Human Resources drawing with dynamic perspective and depth effects"},
            {"Grupo 6", "Turn the image into a professional lawyer in opulent style incorporate justness items"},
            {"Grupo 7", "Convert the image a teacher, like a child’s drawing incorporate education items Turn the image into a teacher in the classroom, as if it were a child's drawing, turn the image a teacher, like a child’s drawing Convert the image to a teacher in classroom"},
            {"Grupo 8", "Convert the image into a psychology digital illustration with crisp lines apply grid art style incorporate psychology and people items"},
            {"Grupo 9", "Convert the image into a photograph with smooth light and motion effects incorporate journalism and mass media items"},
            {"Grupo 10", "Create an artistic collage using the image and other visuals to incorporate marketing and publicity items"},
            {"Grupo 11", "Give the image an industrial design painting look with precise details and clean lines to incorporate industrial design elements around"},
            {"Grupo 12", "Give the image a futuristic look with technological and abstract elements"},
            {"Grupo 13", "Give the image a stylish and sophisticated fashion illustration look"},
            {"Grupo 14", "Convert the image into a detailed and precise financial an mathematical illustration+to incorporate numbers and mathematical items"},
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
                .Add(() =>
                {
                }).Wait(() => readyToNextStep).Add(ShowNextButton);
            flow.Play();
        }
    }
}