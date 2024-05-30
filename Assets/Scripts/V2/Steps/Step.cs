using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace V2.Steps
{
    public abstract class Step : MonoBehaviour, IStep
    {
        [SerializeField] protected StepsConfig stepsConfig;
        [SerializeField] protected Button nextStepButton;

        [SerializeField, InterfaceType(typeof(IStep))]
        private Object nextStep;

        protected IStep NextStep => nextStep as IStep;

        protected virtual void NextAction()
        {
            NextStep.StartStep();
        }

        protected virtual void Start()
        {
            nextStepButton.onClick.AddListener(() =>
            {
                Debug.Log("Next step");
                stepsConfig.NextStep();
                NextAction();
            });
            HideNextButton();
        }

        protected void HideNextButton()
        {
            nextStepButton.gameObject.SetActive(false);
        }

        protected void ShowNextButton()
        {
            nextStepButton.gameObject.SetActive(true);
        }

        public abstract void StartStep();
    }
}