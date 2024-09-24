using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private float interval; // ステージ選択の間隔
    int selectNumber = 0; // 選択されているステージの番号
    bool IsMoving = false; // ステージ選択が移動中かどうか
    float t = 0.0f; // 補間のための時間
    int p0; // 現在の位置
    [SerializeField] Vector3 startPos; // スタート位置
    //[SerializeField] private List<string> Stagelist; // ステージのリスト
    [SerializeField] private SceneChange sceneChange;
    bool IsNoSelect = false; // 何も選んでいない
    float p;
    bool IsPause;
    bool IsSelected; // 選択ずみ

    [SerializeField] private Animator animator;
    [SerializeField] private FooterUI footer;
    [SerializeField] private string titleScene;
    [SerializeField] private GameObject PauseCanvas;
    [SerializeField] private GameObject SettingCanvas;
    [SerializeField] private UnityEngine.UI.Button button;
    private UnityEngine.UI.Button lastSelectedButton;
    [SerializeField] private string[] sl = { "easy", "hard" };

    //[SerializeField] private List<TextMeshProUGUI> lampCounters;

    [System.Serializable]
    public struct StageData
    {
        [SerializeField] private int stageID;
        [SerializeField] private string stageName;
        [SerializeField] private string stageScene;
        [SerializeField] private int maxLampCount;
        public TextMeshProUGUI StageNameTMP;
        public TextMeshProUGUI StageModeTMP;
        [SerializeField] private List<DisplayLamp> lampList;
        public GameObject canvas;
        public GameObject stoveFire;

        public int StageID => stageID;
        public string StageName => stageName;
        public string StageScene => stageScene;
        public int MaxLampCount => maxLampCount;
        public List<DisplayLamp> LampList => lampList;
    }
    [SerializeField] private List<StageData> stageList;


    AudioSource SE;
    AudioSource BGM;

    [SerializeField] private AudioClip StageSelectSE;//ステージを選択した際の音
    [SerializeField] private AudioClip StageSelectBGM;//ステージを選択画面のBGM
    [SerializeField] private AudioClip PauseSE;//ポーズを押した時の音
    [SerializeField] private AudioClip ButtonSE;//ボタンを押した際の音

    void Start()
    {
        gameStageSelectBGM();
        SE = GetComponent<AudioSource>();
        SE.volume = 0.1f;
        BGM = GetComponent<AudioSource>();
        if (SceneSelectionState.selectedIndex == -1)
        {
            p0 = 0; // 初期位置を0に設定
            selectNumber = 0; // 選択番号を設定
            //sceneChange.StartFadeIn();
            IsNoSelect = true;
            p = transform.position.x;
            animator.SetBool("Moving", true);
            footer.GrayOutInstructionTexts();
            SceneSelectionState.mode = 0;
        }
        else
        {
            p0 = SceneSelectionState.selectedIndex; // 前回の選択位置を取得
            selectNumber = p0; // 選択番号を設定
            IsNoSelect = false;
            Vector3 pos = startPos;
            pos.x = startPos.x + p0 * interval; // 選択位置に応じてX座標を調整
            transform.position = pos;
            //sceneChange.StartFadeIn();
            animator.SetBool("Moving", false);
            footer.ActivateInstructionTexts();
            stageList[selectNumber].canvas.SetActive(true);
        }

        IsMoving = false; // 移動中フラグをリセット

        Save.LoadGame();

        for (int i = 0; i < stageList.Count; i++)
        {
            UpdateCanvas(i);
        }
        IsPause = false;
        IsSelected = false;
    }

    void Update()
    {
        if (IsNoSelect)
        {
            t += Time.deltaTime; // 補間の時間を更新
            if (t >= 1.0f)
            {
                t = 1.0f; // 補間時間の上限を1.0に設定
                IsNoSelect = false; // 移動中フラグをリセット
                animator.SetBool("Moving", false);
                footer.ActivateInstructionTexts();
                //transform.position += new Vector3(0.0f, 0.1f, 0.0f);
                stageList[selectNumber].canvas.SetActive(true);
            }
            Vector3 pos = transform.position;
            pos.x = Mathf.Lerp(p, startPos.x, t); // 補間を用いてX座標を計算
            transform.position = pos; // 新しい位置を設定
            return;
        }

        if (!IsMoving && !IsPause)
        {
            // 右キーが押された場合
            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("JoyHorizontal") > 0.1f) && selectNumber + 1 < stageList.Count)
            {
                Move(1);
            }
            // 左キーが押された場合
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("JoyHorizontal") < -0.1f) && 0 < selectNumber)
            {
                Move(-1);
            }
            //難易度切り替え
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("JoyVertical") > 0.1f))
            {
                SceneSelectionState.mode = 1;
                for (int i = 0; i < stageList.Count; i++)
                {
                    UpdateCanvas(i);
                }
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("JoyVertical") < -0.1f))
            {
                SceneSelectionState.mode = 0;
                for (int i = 0; i < stageList.Count; i++)
                {
                    UpdateCanvas(i);
                }
            }

            // エンターキーが押された場合
            if (!IsSelected && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton2)) && 0 <= selectNumber && selectNumber < stageList.Count && stageList[selectNumber].StageName != "")
            {
                gameStageSelectSE();
                SceneSelectionState.selectedIndex = selectNumber; // 現在の選択番号を保存
                SceneSelectionState.IsPlayed = false;
                sceneChange.StartFadeOut(stageList[selectNumber].StageScene);
                IsSelected = true;
            }
        }
        else
        {
            t += Time.deltaTime; // 補間の時間を更新
            if (t >= 1.0f || p0 == selectNumber)
            {
                t = 1.0f; // 補間時間の上限を1.0に設定
                IsMoving = false; // 移動中フラグをリセット
                p0 = selectNumber; // 現在の位置を更新
                animator.SetBool("Moving", false);
                footer.ActivateInstructionTexts();
                //transform.position += new Vector3(0.0f, 0.1f, 0.0f);
                stageList[selectNumber].canvas.SetActive(true);
            }
            Vector3 pos = transform.position;
            pos.x = startPos.x + Mathf.Lerp(p0 * interval, selectNumber * interval, t); // 補間を用いてX座標を計算
            transform.position = pos; // 新しい位置を設定
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

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            if (IsPause)
            {
                CloseMenu();
            }
            else
            {
                IsPause = true;
                PauseCanvas.SetActive(true);
                EventSystem.current.SetSelectedGameObject(button.gameObject);
                gamePauseSE();
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Save.Reset();
            for (int i = 0; i < stageList.Count; i++)
            {
                UpdateCanvas(i);
            }
        }
    }

    public void Move(int delta)
    {
        stageList[selectNumber].canvas.SetActive(false);
        selectNumber += delta; // 選択番号を増やす
        IsMoving = true; // 移動中フラグを設定
        t = 0.0f; // 補間の時間をリセット
        animator.SetBool("Moving", true);
        footer.GrayOutInstructionTexts();
        //transform.position -= new Vector3(0.0f, 0.1f, 0.0f);
        Vector3 ls = transform.localScale;
        if (delta > 0) ls.x = 1.0f;
        else ls.x = -1.0f;
        transform.localScale = ls;
    }

    private void UpdateCanvas(int stageNumber)
    {
        stageList[stageNumber].StageNameTMP.text = stageList[stageNumber].StageName;
        stageList[stageNumber].StageModeTMP.text = sl[SceneSelectionState.mode];
        foreach (DisplayLamp lamp in stageList[stageNumber].LampList)
        {
            lamp.Extinguishment();
        }
        for (int i = 0; i < Save.saveData.lampCounts[stageList[stageNumber].StageID * 2 + SceneSelectionState.mode]; i++)
        {
            stageList[stageNumber].LampList[i].Ignition();
        }
        GameObject stove = stageList[stageNumber].stoveFire;
        if (Save.saveData.clearedList[stageList[stageNumber].StageID * 2 + SceneSelectionState.mode])
        {
            stove.SetActive(true);
            if (Save.saveData.lampCounts[stageList[stageNumber].StageID * 2 + SceneSelectionState.mode] == stageList[stageNumber].MaxLampCount)
                stove.transform.localScale = new Vector3(1f, 1f, 1f);
            else
                stove.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            stove.SetActive(false);
        }
    }

    public void OpenSetting()
    {
        SettingCanvas.SetActive(true);
        PauseCanvas.SetActive(false);
    }
    public void CloseMenu()
    {
        IsPause = false;
        PauseCanvas.SetActive(false);
        SettingCanvas.SetActive(false);
        gameButtonSE();
    }

    public void GoTitle()
    {
        sceneChange.StartFadeOut(titleScene);
    }

    void gameStageSelectSE()
    {
        SE.volume = 0.1f;
        SE.PlayOneShot(StageSelectSE);
    }
    void gameStageSelectBGM()
    {
        GameObject state = GameObject.Find("EventSystem");
        AudioSource audioSource = state.AddComponent<AudioSource>();
        BGM = audioSource;
        BGM.clip = StageSelectBGM;
        BGM.volume = 0.06f;
        BGM.Play();
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
