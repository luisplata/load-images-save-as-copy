using UnityEngine;

public class SaveAndLoadData : ISaveData, ILoadData
{
    public SaveAndLoadData()
    {
        
    }
    
    public static void SaveData(string key, string data)
    {
        PlayerPrefs.SetString(key, data);
    }

    public static string LoadData(string key)
    {
        return PlayerPrefs.GetString(key);
    }
}

public interface ISaveData
{
}

public interface ILoadData
{
}