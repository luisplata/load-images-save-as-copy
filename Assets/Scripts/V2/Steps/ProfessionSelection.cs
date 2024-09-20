using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfessionSelection : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] TextMeshProUGUI text;
    [TextArea(5, 20)]
    [SerializeField] private string name;
    [SerializeField] private string profession;

    private void Reset()
    {
        button = GetComponent<Button>();
        
    }

    public void Configure(Action<string> action)
    {
        button.onClick.AddListener(() =>
        {
            action?.Invoke(profession);
        });
        
        text.text = name;
    }
}