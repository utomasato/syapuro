using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverCanvas;

    [SerializeField]
    private GameObject Background;
    [SerializeField]
    private GameObject GameOverText;
    [SerializeField]
    private GameObject ScoreParents;//スコア表示の親オブジェクト
    [SerializeField]
    private TMP_Text Score_Text;//スコア結果を表示するテキスト
    [SerializeField]
    private GameObject Button;

    private bool IsGameOver = false;

    [SerializeField]
    private GameObject PL;

    private Vector3 SaveScale;
    private Vector3 currentScale;

    private bool IsgameStart = false;

    private float ResultStartTime = 0.0f;//結果を出すまでの時間

    [SerializeField]
    private float ResultEndTime;//この時間を超えると結果を出す

    GameObject[] GameOverAssets;

    private Coroutine SampleCoroutine = null;
    // Start is called before the first frame update
    void Start()
    {
        SaveScale = PL.transform.localScale;
        GameOverAssets = new GameObject[] { GameOverText, ScoreParents, Button };
        foreach (GameObject asset in GameOverAssets)
        {
            asset.SetActive(false);
        }
        Background.SetActive(false);
    }

    // Update is called once per frame

    private int i = 0;
    void Update()
    {

        //  Invoke("Test", 2.0f);
        GameOverSystem();
    }



    void GameOverSystem()
    {
        GameState_test state = GetComponent<GameState_test>();

        if (PL != null)
        {
            currentScale = PL.transform.localScale;
        }

        if (!state.JudgeGameOver && currentScale.y <= 0)
        {

            state.JudgeGameOver = true;
        }


        if (state.JudgeGameOver)
        {
            GameOverCanvas.SetActive(true);
            if (SampleCoroutine == null)
            {

                SampleCoroutine = StartCoroutine(GameCoroutine(2.0f, GameOverAssets));
            }

            Background.SetActive(true);
            ScoreResult(ResultEndTime);
        }
        if (!state.JudgeGameOver)
        {
            ResultStartTime = 0.0f;
            GameOverCanvas.SetActive(false);
        }
    }

    public int RandomScore(int min, int max)
    {
        return Random.Range(min, max);
    }

    public void ScoreResult(float Endtime)
    {
        ResultStartTime += Time.deltaTime;
        if (ResultStartTime < Endtime)
        {
            int score = RandomScore(500, 1500);
            Score_Text.text = score.ToString();
        }
        else
        {
            Score_Text.text = "200";//テスト用
        }
    }


    public IEnumerator GameCoroutine(float delay, GameObject[] Asset)
    {
        GameState_test state = GetComponent<GameState_test>();
        while (i < Asset.Length)
        {

            Asset[i].SetActive(true);
            i++;
            yield return new WaitForSeconds(delay);
        }
        //state.NotGameOver();
        SampleCoroutine = null;
    }

    public void PressRetry()//リトライボタンを押した時
    {
        GameState_test state = GetComponent<GameState_test>();
        transform.localScale = SaveScale;
        state.JudgeGameOver = false;
    }
}



