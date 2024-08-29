using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Keys
{
    public string dashKay;
    public string jumpKay;
    public string transferKay;
    public string pauseKay;

    public Keys(string d, string j, string t, string p)
    {
        dashKay = d;
        jumpKay = j;
        transferKay = t;
        pauseKay = p;
    }
}

public class KeyBindings : MonoBehaviour
{
    private static string configFilePath;
    public static Keys keys;

    public static string DashKay => keys.dashKay;
    public static string JumpKay => keys.jumpKay;
    public static string TransferKay => keys.transferKay;
    public static string PauseKay => keys.pauseKay;

    public static void SaveConfig()
    {
        if (configFilePath == null)
            configFilePath = Application.persistentDataPath + "/configuration.json";
        string json = JsonUtility.ToJson(keys);
        System.IO.File.WriteAllText(configFilePath, json);
        Debug.Log("Config saved to " + configFilePath);
    }

    public static void LoadConfig()
    {
        if (configFilePath == null)
            configFilePath = Application.persistentDataPath + "/configuration.json";
        if (System.IO.File.Exists(configFilePath))
        {
            string json = System.IO.File.ReadAllText(configFilePath);
            Keys k = JsonUtility.FromJson<Keys>(json);
            Debug.Log("Game loaded from " + configFilePath);
            keys = k;
            return;
        }
        else
        {
            Debug.LogWarning("Save file not found at " + configFilePath);
            keys = new Keys("shift", "space", "return", "escape");
            SaveConfig();
            return;
        }
    }
}
