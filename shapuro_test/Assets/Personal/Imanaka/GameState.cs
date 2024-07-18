using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private bool IsGameStart;
    private bool IsGameOver;

    private bool IsGameClear;

    private int score;
    // Start is called before the first frame update
    void Start()
    {
        IsGameStart = false;
        IsGameOver = false;
        IsGameClear = false;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

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
