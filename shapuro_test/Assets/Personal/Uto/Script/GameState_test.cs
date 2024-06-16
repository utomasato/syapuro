using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_test : MonoBehaviour
{
    private int score;
    private int apparentScore;
    private bool IsStarted;
    private bool IsCleared;
    private bool IsGameOver;

    private bool IsAddingUp;

    void Start()
    {
        IsStarted = true;
        IsCleared = false;
        IsGameOver = false;
        score = 0;
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

    public void NotGameOver()
    {
        IsGameOver = false;
    }
    public bool JudgeGameOver
    {
        get { return IsGameOver; }
        set { IsGameOver = value; }
    }


}
