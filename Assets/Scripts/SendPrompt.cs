using System;
using UnityEngine;

public class SendPrompt : MonoBehaviour
{
    [SerializeField] private string uri;
    
    public void SendPromptHttp(Action<string> actionOk, Action actionError)
    {
        
    }
}