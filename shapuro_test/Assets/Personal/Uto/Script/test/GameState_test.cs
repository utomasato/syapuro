using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_test : MonoBehaviour
{
    private int score;

    private int NumberOfCaldles;//ロウソクをつけた個数
    private int apparentScore;
    //private bool IsStarted;
    private bool IsCleared;
    private bool IsGameOver;

    private bool IsAddingUp;

    void Start()
    {
        //IsStarted = true;
        IsCleared = false;
        IsGameOver = false;
        score = 0;
        NumberOfCaldles = 0;
    }

    void Update()
    {

        if (IsAddingUp)
        {
            apparentScore += 1;
            if (apparentScore >= score)
            {
                apparentScore = score;
                IsAddingUp = false;
            }
        }

    }

    public void AddScore(int addscore)
    {
        score += addscore;
        NumberOfCaldles++;//今中追加
        IsAddingUp = true;
        Debug.Log(score);
    }

    public int GetScore()
    {
        return score;
    }

    public void GameOver()
    {
        IsGameOver = true;
    }

    public void Clear()
    {
        IsCleared = true;
    }
    public bool JudgeGameOver
    {
        get { return IsGameOver; }
        set { IsGameOver = value; }
    }
    public bool JudgeGameClear
    {
        get { return IsCleared; }
        set { IsCleared = value; }
    }
    public int JudgeNumberCaldles//蝋燭をつけた個数
    {
        get { return NumberOfCaldles; }
        set { NumberOfCaldles = value; }
    }
    public int JudgecurrentScore
    {
        get { return score; }
        set { score = value; }
    }
}
