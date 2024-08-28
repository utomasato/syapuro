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

    private int score;
    private int LampCount = 0;
    [SerializeField]
    private int MaxLampPerStage = 0;//ステージごとのランプ個数

    [SerializeField] private Fire plyer;
    [SerializeField] private GaugeController gaugeController;
    [SerializeField] private GameClear gameClear;
    [SerializeField] private GameOver gameOver;
    [SerializeField] private Pause pause;
    [SerializeField] private FooterUI footer;
    [SerializeField] private SceneChange sceneChange;
    [SerializeField] private string selectScene;
    private bool buttonClicked;

    /*AudioSourceを2つにして、片方をSEに、もう片方をBGMに入れてください*/
    [SerializeField] AudioSource SE;
    [SerializeField] AudioSource BGM;
    [SerializeField] private AudioClip GameBGM;//プレイ中のBGM
    [SerializeField] private AudioClip LampSE;//ランプをつけた時の効果音
    [SerializeField] private AudioClip GameOverSE;
    [SerializeField] private AudioClip GameClearSE;
    [SerializeField] private AudioClip PauseSE;//ポーズを押した時の音
    [SerializeField] private AudioClip ButtonSE;//何らかのボタンを押した時の音
    private UnityEngine.UI.Button lastSelectedButton;

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
    private List<bool> BurningLampList;

    //[SerializeField] private StagePlayData playData;

    void Start()
    {
        state = State.BeforeStart;
        score = 0;
        if (SE == null)
        {
            SE = gameObject.AddComponent<AudioSource>();
        }
        SE = GetComponent<AudioSource>();
        SE.volume = 0.5f;
        footer.GrayOutInstructionTexts();
        buttonClicked = false;
        BurningLampList = new List<bool>();
        for (int i = 0; i < MaxLampPerStage; i++)
        {
            BurningLampList.Add(false);
        }
    }

    void Update()
    {
        if (state == State.GamePlay)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // ポーズ画面にする
            {
                Pause();
            }
        }
        else if (state != State.Explain)
        {
            if (state == State.Pause && Input.GetKeyDown(KeyCode.Escape)) // ポーズ解除
            {
                Resume();
                return;
            }
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

    public void AddLampCount(int LampID = -1)//20240809 宇藤追加
    {
        LampCount++;
        gaugeController.UpdateCount(LampCount);
        gameLampSE();
        if (LampID != -1)
            BurningLampList[LampID] = true;
    }

    public List<bool> GetBurningLampList
    {
        get { return BurningLampList; }
    }

    public void GameStart()//20240810uto
    {
        gameBGM();
        state = State.GamePlay;
        footer.ActivateInstructionTexts();
    }

    public void Pause()//20240810uto
    {
        if (state == State.Pause) return;
        beforePauseState = state;
        state = State.Pause;
        gamePauseSE();
        plyer.UseCandle().Pause();
        pause.PauseSystem();
    }

    public void Resume()//20240810uto
    {
        // ポーズでの選択のEnter/Spaceが火の玉/ジャンプにならないように1フレーム遅らせる
        StartCoroutine(ExecuteAfterOneFrame(() =>
        {
            gameButtonSE();
            state = beforePauseState;
            plyer.UseCandle().Resume();
            pause.ResumeSystem();
            lastSelectedButton = null;
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
        BGM.Stop();
        gameClearSE();
        gameClear.ClearSystem();

        if (SceneSelectionState.selectedIndex != -1)
        {
            List<int> list = Save.saveData.lampCounts;
            if (list[SceneSelectionState.selectedIndex] < LampCount)
            {
                list[SceneSelectionState.selectedIndex] = LampCount;
                Save.saveData.lampCounts = list;
                Save.SaveGame();
            }
        }
    }

    public void GameOver()//20240810uto
    {
        if (state == State.GameOver) return;
        state = State.GameOver;
        BGM.Stop();
        gameOverSE();
        gameOver.GameOverSystem();
    }

    public bool JudgeState(string targetState)//20240810uto
    {
        //Debug.Log(state);
        return state.ToString() == targetState;
    }

    public void SetButton(UnityEngine.UI.Button setButton = null)
    {
        EventSystem.current.SetSelectedGameObject(setButton.gameObject);
    }

    /*
    public bool JudgeCountdown//スタート時、カウントダウンが始まっているか
    {
        get { return IsCountdown; }
        set { IsCountdown = value; }
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



    public void Retry() // シーンをリロードする
    {
        if (!buttonClicked)
        {
            gameButtonSE();
            sceneChange.StartFadeOut(SceneManager.GetActiveScene().name);
            buttonClicked = true;
        }
    }

    public void ExitStage() // セレクト画面に戻る
    {
        if (!buttonClicked)
        {
            gameButtonSE();
            sceneChange.StartFadeOut(selectScene);
            buttonClicked = true;
        }
    }

    IEnumerator ExecuteAfterOneFrame(Action action) // 実行を１フレーム遅らせる
    {
        yield return null; // 1フレーム待つ
        action?.Invoke(); // 渡された処理を実行
    }

    public void gameBGM()
    {
        BGM.clip = GameBGM;
        BGM.volume = 0.1f;
        BGM.Play();
    }
    public void gameLampSE()
    {
        SE.volume = 0.15f;
        SE.PlayOneShot(LampSE);
    }
    public void gameOverSE()
    {
        SE.volume = 0.5f;
        SE.PlayOneShot(GameOverSE);
    }
    public void gameClearSE()
    {
        SE.volume = 0.15f;
        SE.PlayOneShot(GameClearSE);
    }
    public void gamePauseSE()
    {
        SE.volume = 0.1f;
        SE.PlayOneShot(PauseSE);
    }
    public void gameButtonSE()
    {
        SE.volume = 0.04f;
        SE.PlayOneShot(ButtonSE);
    }
}

