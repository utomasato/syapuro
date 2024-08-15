using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    [SerializeField] private Pause pause;
    [SerializeField] private SceneChange sceneChange;
    [SerializeField] private string selectScene;

    private UnityEngine.UI.Button lastSelectedButton;
    //private List<UnityEngine.UI.Button> buttons = null; // ボタンのリスト
    //private int currentIndex = 0; // 現在選択されているボタンのインデックス
    //private RectTransform selectPointer;

    public enum State
    {
        BeforeStart,
        GamePlay,
        Explain,
        Pause,
        GameClear,
        GameOver
    }
    [SerializeField] private State state; // ゲームの状態を管理する
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

    void Update()
    {
        if (state == State.Pause)
        {
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject; // 現在選択されているUI要素を取得

            // 現在選択されているものがボタンでない場合、最後に選択されたボタンを再選択する
            if (currentSelected == null || currentSelected.GetComponent<UnityEngine.UI.Button>() == null)
            {
                if (lastSelectedButton != null)
                {
                    EventSystem.current.SetSelectedGameObject(lastSelectedButton.gameObject);
                }
            }
            else // 現在選択されているものがボタンであれば、それを記憶
            {
                lastSelectedButton = currentSelected.GetComponent<UnityEngine.UI.Button>();
            }
        }
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
        if (state == State.Pause) return;
        beforePauseState = state;
        state = State.Pause;
        plyer.UseCandle().Pause();
        pause.PauseSystem();
    }

    public void Resume()//20240810uto
    {
        StartCoroutine(ExecuteAfterOneFrame(() =>
        {
            state = beforePauseState;
            plyer.UseCandle().Resume();
            pause.ResumeSystem();
            //button = null;
        }));

    }

    public void OpenExplain()//20240811uto
    {
        beforePauseState = state;
        state = State.Explain;
        plyer.UseCandle().Pause();
    }

    public void CloseExplain()//20240811uto
    {
        state = beforePauseState;
        plyer.UseCandle().Resume();
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

    public void SetButton(UnityEngine.UI.Button setButton)
    {
        EventSystem.current.SetSelectedGameObject(setButton.gameObject);
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

    public void Retry() // シーンをリロードする
    {
        sceneChange.StartFadeOut(SceneManager.GetActiveScene().name);
    }

    public void ExitStage() // セレクト画面に戻る
    {
        sceneChange.StartFadeOut(selectScene);
    }

    IEnumerator ExecuteAfterOneFrame(Action action) // 実行を１フレーム遅らせる
    {
        yield return null; // 1フレーム待つ
        action?.Invoke(); // 渡された処理を実行
    }
}

