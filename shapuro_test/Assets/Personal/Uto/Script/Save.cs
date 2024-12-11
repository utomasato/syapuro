/*
LoadGame() : jsonから取得したものをsaveDataに格納する
SaveGame() : saveDataをjsonに反映する
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SaveData
{
    [SerializeField]
    private List<int> LampCounts;
    [SerializeField]
    private List<bool> ClearedList;
    public SaveData(List<int> num, List<bool> tf)
    {
        this.LampCounts = num;
        this.ClearedList = tf;
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
    public List<bool> clearedList
    {
        set
        {
            this.ClearedList = value;
            Debug.Log(string.Join(", ", ClearedList));
        }
        get { return this.ClearedList; }
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
        else // セーブデータが見つからないとき
        {
            Debug.LogWarning("Save file not found at " + saveFilePath);
            Reset();
            return;
        }
    }

    public static void Reset()
    {
        saveData = new SaveData(
            new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<bool>() { false, false, false, false, false, false, false, false, false, false, false, false,
                                false, false, false, false, false, false, false, false, false, false, false, false }
            );
        SaveGame();
    }

}
