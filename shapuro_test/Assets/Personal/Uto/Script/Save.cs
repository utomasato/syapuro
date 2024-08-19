using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SaveData
{
    [SerializeField]
    private List<int> LampCounts;
    public SaveData(List<int> num)
    {
        this.LampCounts = num;
        Debug.Log("reset");
        Debug.Log(string.Join(", ", LampCounts));
    }

    public List<int> lampCounts
    {
        set
        {
            this.LampCounts = value;
            Debug.Log(string.Join(", ", LampCounts));
        }
        get { return this.LampCounts; }
    }
}

public class Save : MonoBehaviour
{
    private static string saveFilePath;

    public static SaveData saveData;

    public static void SaveGame()
    {
        if (saveFilePath == null)
            saveFilePath = Application.persistentDataPath + "/saveData.json";
        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved to " + saveFilePath);
    }

    public static void LoadGame()
    {
        if (saveFilePath == null)
            saveFilePath = Application.persistentDataPath + "/saveData.json";
        if (System.IO.File.Exists(saveFilePath))
        {
            string json = System.IO.File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded from " + saveFilePath);
            saveData = data;
            return;
        }
        else
        {
            Debug.LogWarning("Save file not found at " + saveFilePath);
            saveData = new SaveData(new List<int>() { 0, 0, 0 });
            SaveGame();
            return;
        }
    }

}
