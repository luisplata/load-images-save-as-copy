using UnityEngine;

public class SaveAndLoadData : ISaveData, ILoadData
{
    public SaveAndLoadData()
    {
        
    }
    
    public void SaveData(string key, string data)
    {
        Debug.Log($"Guardando datos en {key} con valor {data}");
        PlayerPrefs.SetString(key, data);
    }

    public string LoadData(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public bool HasData(string key)
    {
        Debug.Log($"Buscando datos en {key}");
        Debug.Log($"El dato {key} existe? {PlayerPrefs.HasKey(key)}");
        Debug.Log($"El dato {key} tiene el valor {PlayerPrefs.GetString(key)}");
        return PlayerPrefs.HasKey(key);
    }
}

public interface ISaveData
{
    void SaveData(string key, string data);
}

public interface ILoadData
{
    string LoadData(string key);
    bool HasData(string key);
}