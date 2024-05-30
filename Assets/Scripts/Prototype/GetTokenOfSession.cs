using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

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
        StartCoroutine(postRequest.SendRequest(url, passwordObject, (d) =>
        {
            actionOk?.Invoke(d.token);
        }, (e) =>
        {
            actionError?.Invoke();
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


public class HttpResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string Body { get; set; }
}

public class HttpPostRequest<T>
{
    public IEnumerator SendRequest(string url, object data, Action<T> onSuccess, Action<string> onError)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";

        string json = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.ContentLength = bodyRaw.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(bodyRaw, 0, bodyRaw.Length);
        }

        try
        {
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Debug.Log(responseString);
                T result = JsonUtility.FromJson<T>(responseString);
                onSuccess?.Invoke(result);
            }
            else
            {
                onError?.Invoke($"Error: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            onError?.Invoke($"Exception: {e.Message}");
        }

        yield return null;
    }
    public IEnumerator SendRequestWithToken(string url, object data, Action<T> onSuccess, Action<string> onError)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";

        request.Headers.Add("Authorization", $"Bearer {SaveAndLoadData.LoadData("token")}");

        string json = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.ContentLength = bodyRaw.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(bodyRaw, 0, bodyRaw.Length);
        }

        var asyncResult = request.BeginGetResponse(null, null);

        while (!asyncResult.IsCompleted)
        {
            yield return null;
        }

        try
        {
            var response = (HttpWebResponse)request.EndGetResponse(asyncResult);
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Debug.Log(responseString);
                var result = JsonUtility.FromJson<T>(responseString);
                onSuccess?.Invoke(result);
            }
            else
            {
                onError?.Invoke($"Error: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            onError?.Invoke($"Exception: {e.Message}");
        }
    }
}