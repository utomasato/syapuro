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

    [SerializeField] private Fire player;
    [SerializeField] private SpriteRenderer GoalFire;
    [SerializeField] private GaugeController gaugeController;
    [SerializeField] private GameClear gameClear;
    [SerializeField] private GameOver gameOver;
    [SerializeField] private Pause pause;
    [SerializeField] private FooterUI footer;
    [SerializeField] private SceneChange sceneChange;
    [SerializeField] private string selectScene;
    [SerializeField] private string nextScene;
    private bool buttonClicked;

    /*AudioSourceを2つにして、片方をSEに、もう片方をBGMに入れてください*/
    [SerializeField] AudioSource SE;
    [SerializeField] AudioSource BGM;
    [SerializeField] private AudioClip GameBGM_Tutorial;//チュートリアルのBGM
    [SerializeField] private AudioClip GameBGM_1;//ステージ１のBGM
    [SerializeField] private AudioClip GameBGM_2;//ステージ2のBGM
    [SerializeField] private AudioClip LampSE;//ランプをつけた時の効果音
    [SerializeField] private AudioClip GameOverSE;
    [SerializeField] private AudioClip GameClearSE;
    [SerializeField] private AudioClip PauseSE;//ポーズを押した時の音
    [SerializeField] private AudioClip ButtonSE;//何らかのボタンを押した時の音
    private UnityEngine.UI.Button lastSelectedButton;
    private GameObject beforePauseButton;

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
        if (state == State.GamePlay || state == State.Explain)
        {
            if (Input.GetKeyDown(KeyBindings.PauseKay) || Input.GetKeyDown(KeyCode.JoystickButton9)) // ポーズ画面にする
            {
                Pause();
            }
        }
        else
        {
            if (state == State.Pause && (Input.GetKeyDown(KeyBindings.PauseKay) || Input.GetKeyDown(KeyCode.JoystickButton9)))// ポーズ解除
            {
                Resume();
                return;
            }
            if (state != State.Explain)
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

        if (SceneManager.GetActiveScene().name == "stage1")
        {
            gameBGM_Stage1();
        }
        if (SceneManager.GetActiveScene().name == "stage2")
        {
            gameBGM_Stage2();
        }
        state = State.GamePlay;
        footer.ActivateInstructionTexts();
    }

    public void Pause()//20240810uto
    {
        if (state == State.Pause) return;
        beforePauseState = state;
        beforePauseButton = EventSystem.current.currentSelectedGameObject;
        state = State.Pause;
        gamePauseSE();
        player.GetCandle().Pause();
        pause.PauseSystem();

    }

    public void Resume()//20240810uto
    {
        // ポーズでの選択のEnter/Spaceが火の玉/ジャンプにならないように1フレーム遅らせる
        StartCoroutine(ExecuteAfterOneFrame(() =>
        {
            gameButtonSE();
            state = beforePauseState;
            player.GetCandle().Resume();
            pause.ResumeSystem();
            lastSelectedButton = null;
            EventSystem.current.SetSelectedGameObject(beforePauseButton);
        }));

    }

    public void OpenExplain()//20240811uto
    {
        beforePauseState = state;
        state = State.Explain;
        player.GetCandle().Pause();

    }

    public void CloseExplain()//20240811uto
    {
        state = beforePauseState;
        player.GetCandle().Resume();

    }

    public void GameClear()//20240810uto
    {
        if (state == State.GameClear) return;
        state = State.GameClear;
        GoalFire.enabled = true;
        BGM.Stop();
        gameClearSE();
        gameClear.ClearSystem();

        if (SceneSelectionState.selectedIndex != -1)
        {
            List<int> list = Save.saveData.lampCounts;
            List<bool> Blist = Save.saveData.clearedList;
            if (list[SceneSelectionState.selectedIndex * 2 + SceneSelectionState.mode] < LampCount)
            {
                list[SceneSelectionState.selectedIndex * 2 + SceneSelectionState.mode] = LampCount;
                Save.saveData.lampCounts = list;
            }
            Blist[SceneSelectionState.selectedIndex * 2 + SceneSelectionState.mode] = true;
            Save.saveData.clearedList = Blist;
            Save.SaveGame();
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

    public void NextStage()
    {
        if (!buttonClicked)
        {
            gameButtonSE();
            sceneChange.StartFadeOut(nextScene);
            buttonClicked = true;
        }
    }

    IEnumerator ExecuteAfterOneFrame(Action action) // 実行を１フレーム遅らせる
    {
        yield return null; // 1フレーム待つ
        action?.Invoke(); // 渡された処理を実行
    }

    public string Rank(TMP_Text text)
    {
        if (MaxLampPerStage == 0)
        {
            text.text = "ー";
        }
        else
        {
            float Judgelamp = (float)LampCount / MaxLampPerStage;
            if (Judgelamp == 1.0f)
            {
                text.text = "S";  // すべてのランプをつけた場合
            }
            else if (Judgelamp >= 0.7f)
            {
                text.text = "A";
            }
            else if (Judgelamp >= 0.5f)
            {
                text.text = "B";
            }
            else if (Judgelamp >= 0.2f)
            {
                text.text = "C";
            }
            else
            {
                text.text = "D";
            }
        }
        return text.text;
    }

    public void TutorialBGM()
    {
        BGM.clip = GameBGM_Tutorial;
        BGM.volume = 0.15f;
        BGM.Play();
    }
    public void gameBGM_Stage1()
    {
        BGM.clip = GameBGM_1;
        BGM.volume = 0.1f;
        BGM.Play();
    }
    public void gameBGM_Stage2()
    {
        BGM.clip = GameBGM_2;
        BGM.volume = 0.08f;
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

