using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private bool IsCountdown;
    private bool IsGameStart;
    private bool IsGameOver;

    private bool IsGameClear;

    private bool IsAddingUp;

    private bool IsExplain;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        IsCountdown = false;
        IsGameStart = false;
        IsGameOver = false;
        IsGameClear = true;
        IsAddingUp = false;
        IsExplain = false;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

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


}
