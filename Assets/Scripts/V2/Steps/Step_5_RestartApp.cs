using UnityEngine.SceneManagement;

namespace V2.Steps
{
    public class Step_5_RestartApp : Step
    {
        public override void StartStep()
        {
            ShowNextButton();
        }

        protected override void NextAction()
        {
           SceneManager.LoadScene(0);
        }
    }
}