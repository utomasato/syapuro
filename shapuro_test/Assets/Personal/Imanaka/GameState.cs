using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private bool IsGameStart;
    private bool IsGameOver;

    private bool IsGameClear;

    private int Score = 0;
    // Start is called before the first frame update
    void Start()
    {
        IsGameStart = false;
        IsGameOver = false;
        IsGameClear = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public int GetScore()
    {
        return Score;
    }
    public void SetGameStart()//ゲームスタートしたか
    {
        IsGameStart = true;
    }
    public bool GetIsGameStart()
    {
        return IsGameStart;
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
}
