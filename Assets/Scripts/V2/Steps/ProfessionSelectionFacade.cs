using System.Collections.Generic;
using UnityEngine;
using V2.Steps;

public class ProfessionSelectionFacade : MonoBehaviour, IProfessionSelection
{
    [SerializeField] private List<ProfessionSelection> professionSelections;
    private Step_2_1_SelectProfession _step21SelectProfession;

    public void Configure(Step_2_1_SelectProfession step21SelectProfession)
    {
        _step21SelectProfession = step21SelectProfession;
        foreach (var professionSelection in professionSelections)
        {
            professionSelection.Configure(value =>
            {
                _step21SelectProfession.CallbackToSelection(value);
            });
        }
    }
}