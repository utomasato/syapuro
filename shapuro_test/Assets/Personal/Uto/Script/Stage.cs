using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    [SerializeField] private int stageID;
    [SerializeField] private string stageScene;
    public GameObject canvas;

    [SerializeField] private string stageName;
    [SerializeField] private int maxLampCount;
    public TextMeshProUGUI StageNameTMP;
    public TextMeshProUGUI StageModeTMP;
    [SerializeField] private List<DisplayLamp> lampList;

    public GameObject stoveFire;

    public int StageID => stageID;
    public string StageName => stageName;
    public string StageScene => stageScene;
    public int MaxLampCount => maxLampCount;
    public List<DisplayLamp> LampList => lampList;

    [SerializeField] private string[] modeList = { "Easy", "Hard" };
    [SerializeField] private TextMeshProUGUI RankTMP;

    public void ShowDisplay()
    {
        canvas.SetActive(true);
    }
    public void HideDisplay()
    {
        canvas.SetActive(false);
    }

    void Start()
    {
        Save.LoadGame();
        if (Save.saveData.lampCounts.Count < (StageID + 1) * 2)
        {
            Save.saveData.lampCounts.Add(0);
            Save.saveData.lampCounts.Add(0);
            Save.saveData.clearedList.Add(false);
            Save.saveData.clearedList.Add(false);
        }
        Save.SaveGame();
    }

    public void UpdateDisplay()
    {
        StageNameTMP.text = StageName; // ステージ名を表示する
        StageModeTMP.text = modeList[SceneSelectionState.mode]; // 難易度を表示する
        int lampCount = Save.saveData.lampCounts[StageID * 2 + SceneSelectionState.mode];
        for (int i = 0; i < MaxLampCount; i++) // ステージの達成度を更新
        {
            if (i < lampCount)
                LampList[i].Ignition();
            else
                LampList[i].Extinguishment();
        }
        GameObject stove = stoveFire;
        if (Save.saveData.clearedList[StageID * 2 + SceneSelectionState.mode]) // クリア済みなら暖炉の火をつける
        {
            stove.SetActive(true);
            if (Save.saveData.lampCounts[StageID * 2 + SceneSelectionState.mode] == MaxLampCount) // クリア状況によって火の大きさを変える
                stove.transform.localScale = new Vector3(1f, 1f, 1f);
            else
                stove.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            RankTMP.text = Rank();
        }
        else
        {
            stove.SetActive(false);
            RankTMP.text = "ー";
        }
    }

    private string Rank()
    {
        string rank = "ー";
        int lampCount = Save.saveData.lampCounts[StageID * 2 + SceneSelectionState.mode];
        if (maxLampCount == 0)
        {
            rank = "ー";
        }
        else
        {
            float Judgelamp = (float)lampCount / maxLampCount;
            if (Judgelamp == 1.0f)
            {
                rank = "S";  // すべてのランプをつけた場合
            }
            else if (Judgelamp >= 0.7f)
            {
                rank = "A";
            }
            else if (Judgelamp >= 0.5f)
            {
                rank = "B";
            }
            else if (Judgelamp >= 0.2f)
            {
                rank = "C";
            }
            else
            {
                rank = "D";
            }
        }
        return rank;
    }
}
