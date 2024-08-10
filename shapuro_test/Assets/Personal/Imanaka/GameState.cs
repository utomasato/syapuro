using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameState : MonoBehaviour
{
    /*
    private bool IsCountdown;
    private bool IsGameStart;
    private bool IsGameOver;
    private bool IsGameClear;
    private bool IsAddingUp;
    private bool IsExplain;
    */
    private int score;
    private int LampCount = 0;
    [SerializeField]
    private int MaxLampPerStage = 0;//ステージごとのランプ個数

    [SerializeField] private Fire plyer;
    [SerializeField] private GaugeController gaugeController;
    [SerializeField] private GameClear gameClear;
    [SerializeField] private GameOver gameOver;

    public enum State
    {
        BeforeStart, //スタート前
        GamePlay,  // スタート後
        Explain, // 説明中
        Pause, // ポーズ中
        GameClear, // クリア
        GameOver // ゲームオーバー
    }
    [SerializeField] private State state;
    private State beforePauseState;

    void Start()
    {
        state = State.BeforeStart;
        /*
        IsCountdown = false;
        IsGameStart = false;
        IsGameOver = false;
        IsGameClear = true;
        IsAddingUp = false;
        IsExplain = false;
        */
        score = 0;

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

    public void GameStart()//20240810uto
    {
        state = State.GamePlay;
    }

    public void Pause()//20240810uto
    {
        beforePauseState = state;
        state = State.Pause;
        plyer.UseCandle().StopAnimation();
    }

    public void Resume()//20240810uto
    {
        state = beforePauseState;
        plyer.UseCandle().PlayAnimation();
    }

    public void GameClear()//20240810uto
    {
        if (state == State.GameClear) return;
        state = State.GameClear;
        gameClear.ClearSystem();
    }

    public void GameOver()//20240810uto
    {
        if (state == State.GameOver) return;
        state = State.GameOver;
        gameOver.GameOverSystem();
    }

    public bool JudgeState(string targetState)//20240810uto
    {
        //Debug.Log(state);
        return state.ToString() == targetState;
    }
    /*
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
    */
    public int GetMaxLamp()
    {
        return MaxLampPerStage;
    }

    public TMP_Text JudgeRank(TMP_Text RankText)
    {

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
