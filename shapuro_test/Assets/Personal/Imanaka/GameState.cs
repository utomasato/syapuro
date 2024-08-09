using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameState : MonoBehaviour
{
    private bool IsCountdown;
    private bool IsGameStart;
    private bool IsGameOver;

    private bool IsGameClear;

    private bool IsAddingUp;

    private bool IsExplain;
    private int score;
    private int LampCount = 0;
    [SerializeField]
    private int MaxLampPerStage = 0;//ステージごとのランプ個数
    [SerializeField] private GaugeController gaugeController;
    // Start is called before the first frame update
    void Start()
    {
        IsCountdown = false;
        IsGameStart = false;
        IsGameOver = false;
        IsGameClear = false;
        IsAddingUp = false;
        IsExplain = false;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(IsGameClear);
    }
    public void AddScore(int addscore)
    {
        score += addscore;
        Debug.Log(score);
    }
    public int GetScore()
    {
        return score;
    }
    public void AddLampCount()//20240809 宇藤追加
    {
        LampCount++;
        gaugeController.UpdateCount(LampCount);
    }
    public void SetGameStart()//ゲームスタートしたか
    {
        IsGameStart = true;
    }
    public bool GetIsGameStart()
    {
        return IsGameStart;
    }


    public bool JudgeCountdown//スタート時、カウントダウンが始まっているか
    {
        get { return IsCountdown; }
        set { IsCountdown = value; }
    }
    public bool JudgeGameOver//ゲームオーバーか判断　ゲッター・セッター
    {
        get { return IsGameOver; }
        set { IsGameOver = value; }
    }
    public bool JudgeGameClear//ゲームクリアか判断　ゲッター・セッター
    {
        get { return IsGameClear; }
        set { IsGameClear = value; }
    }
    public bool JudgeExplain//チュートリアル説明中か
    {
        get { return IsExplain; }
        set { IsExplain = value; }
    }
    public int GetMaxLamp()
    {
        return MaxLampPerStage;
    }
    public TMP_Text JudgeRank()
    {
        TMP_Text RankText = null;
        if (MaxLampPerStage == LampCount)
        {
            RankText.text = "S";
        }
        else if (LampCount >= (int)MaxLampPerStage * 0.8)//ランプ8割つけることができれば
        {
            RankText.text = "A";
        }
        else if (LampCount >= (int)MaxLampPerStage * 0.6)
        {
            RankText.text = "B";
        }
        else if (LampCount >= (int)MaxLampPerStage * 0.3)
        {
            RankText.text = "C";
        }
        else
        {
            RankText.text = "D";
        }
        return RankText;
    }
}
