using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GetTokenOfSession : MonoBehaviour
{
    [SerializeField] private string uri;
    [SerializeField] private string password;


    public void PostRequest(string endpoint, Action<string> actionOk, Action actionError)
    {
        var url = endpoint + uri;
        Debug.Log("URL: " + url);
        var passwordObject = new PasswordData
        {
            password = password
        };
        var postRequest = new HttpPostRequest<TokenData>();
        StartCoroutine(postRequest.SendRequestWithToken(url, passwordObject, (d) => { actionOk?.Invoke(d.token); },
            (e) =>
            {
                actionError?.Invoke();
                Debug.Log(e);
            }));
    }
}

[Serializable]
public class PasswordData
{
    public string password;
}

[Serializable]
public class TokenData
{
    public string token;
}


public class HttpPostRequest<T>
{
    public IEnumerator SendRequestWithToken(string url, object data, Action<T> onSuccess, Action<string> onError,
        int maxRetries = 3)
    {
        int attempts = 0;
        bool requestSucceeded = false;

        while (attempts < maxRetries && !requestSucceeded)
        {
            var json = JsonUtility.ToJson(data);
            var request = new UnityWebRequest(url, "POST")
            {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json)),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {SaveAndLoadData.LoadData("token")}");
            request.timeout = 60;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
                T result = JsonUtility.FromJson<T>(request.downloadHandler.text);
                onSuccess?.Invoke(result);
                requestSucceeded = true;
            }
            else if (request.result == UnityWebRequest.Result.ConnectionError ||
                     request.result == UnityWebRequest.Result.ProtocolError)
            {
                if (request.error == "Request timeout")
                {
                    attempts++;
                    Debug.Log($"Attempt {attempts} failed with timeout. Retrying...");
                }
                else
                {
                    yield return new WaitForSeconds(1);
                    onError?.Invoke($"Error: {request.error}");
                    break;
                }
            }
            else
            {
                yield return new WaitForSeconds(1);
                onError?.Invoke($"Error: {request.error}");
                break;
            }

            if (attempts >= maxRetries)
            {
                yield return new WaitForSeconds(1);
                onError?.Invoke("Request timeout. Maximum retry attempts reached.");
            }
        }
    }
}