using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private float interval; // ステージ選択の間隔
    int selectNumber = 0; // 選択されているステージの番号
    bool IsMoving = false; // ステージ選択が移動中かどうか
    float t = 0.0f; // 補間のための時間
    int p0; // 現在の位置
    [SerializeField] Vector3 startPos; // スタート位置
    [SerializeField] private List<string> Stagelist; // ステージのリスト
    [SerializeField] private SceneChange sceneChange;
    bool IsNoSelect = false;
    float p;

    [SerializeField] private Animator animator;
    [SerializeField] private FooterUI footer;
    [SerializeField] private string titleScene;

    [SerializeField] private List<TextMeshProUGUI> textList;

    //[SerializeField] private List<StagePlayData> PlayDatas;

    AudioSource SE;

    AudioSource BGM;

    [SerializeField] private AudioClip StageSelectSE;//ステージを選択した際の音
    [SerializeField] private AudioClip StageSelectBGM;//ステージを選択画面のBGM
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
            sceneChange.StartFadeIn();
            IsNoSelect = true;
            p = transform.position.x;
            animator.SetBool("Moving", true);
            footer.GrayOutInstructionTexts();
            /*
            StaticSave.stagePlayDatas.Clear();
            foreach (StagePlayData data in PlayDatas)
            {
                StaticSave.stagePlayDatas.Add(data);
            }
            */
        }
        else
        {
            p0 = SceneSelectionState.selectedIndex; // 前回の選択位置を取得
            selectNumber = p0; // 選択番号を設定
            IsNoSelect = false;
            Vector3 pos = startPos;
            pos.x = startPos.x + p0 * interval; // 選択位置に応じてX座標を調整
            transform.position = pos;
            sceneChange.StartFadeIn();
            animator.SetBool("Moving", false);
            footer.ActivateInstructionTexts();
            //transform.position += new Vector3(0.0f, 0.1f, 0.0f);
        }

        IsMoving = false; // 移動中フラグをリセット

        Save.LoadGame();
        for (int i = 0; i < Stagelist.Count; i++)
        {
            textList[i].text = Save.saveData.lampCounts[i].ToString();
        }
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
            }
            Vector3 pos = transform.position;
            pos.x = Mathf.Lerp(p, startPos.x, t); // 補間を用いてX座標を計算
            transform.position = pos; // 新しい位置を設定
            return;
        }

        if (!IsMoving)
        {
            // 右キーが押された場合
            if (Input.GetKeyDown(KeyCode.D) && selectNumber + 1 < Stagelist.Count)
            {
                Move(1);
            }
            // 左キーが押された場合
            if (Input.GetKeyDown(KeyCode.A) && 0 < selectNumber)
            {
                Move(-1);
            }

            // エンターキーが押された場合
            if (Input.GetKeyDown(KeyCode.Return) && 0 <= selectNumber && selectNumber < Stagelist.Count && Stagelist[selectNumber] != null)
            {
                gameStageSelectSE();
                SceneSelectionState.selectedIndex = selectNumber; // 現在の選択番号を保存
                sceneChange.StartFadeOut(Stagelist[selectNumber]);
                //SceneManager.LoadScene(Stagelist[selectNumber]); // 選択されたステージをロード
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
            }
            Vector3 pos = transform.position;
            pos.x = startPos.x + Mathf.Lerp(p0 * interval, selectNumber * interval, t); // 補間を用いてX座標を計算
            transform.position = pos; // 新しい位置を設定
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneChange.StartFadeOut(titleScene);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            List<int> list = Save.saveData.lampCounts;
            list[selectNumber] = 0;
            Save.saveData.lampCounts = list;
            Save.SaveGame();
            for (int i = 0; i < Stagelist.Count; i++)
            {
                textList[i].text = Save.saveData.lampCounts[i].ToString();
            }
        }
    }

    public void Move(int delta)
    {
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

    void gameStageSelectSE()
    {
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
}
