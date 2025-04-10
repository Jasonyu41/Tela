using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static void SaveByJson(string saveFileName, object data, bool prettyPrint = false)
    {
        var json = JsonUtility.ToJson(data, prettyPrint);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.WriteAllText(path, json);

            #if UNITY_EDITOR
            Debug.Log($"Susscessfully saved data to {path}.");
            #endif
        }
        catch (System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"Failed to save data to {path}. \n{exception}");
            #endif
        }
    }

    public static T LoadFromJson<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);

            return data;
        }
        catch (System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"Failed to load data to {path}. \n{exception}");
            #endif

            return default;
        }
    }

    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.Delete(path);
        }
        catch (System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"Failed to delete {path}. \n{exception}");
            #endif
        }
    }
}
