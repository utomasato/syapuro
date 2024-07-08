using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverCanvas;//ゲームオーバーCanvasの親オブジェクト

    [SerializeField]
    private GameObject Background;//Image
    [Tooltip("GAMEOVERと書かれているテキストを入れてください")]
    [SerializeField]
    private GameObject GameOverText;
    [SerializeField]
    private GameObject ScoreParents;//スコア表示の親オブジェクト
    [SerializeField]
    private TMP_Text Score_Text;//スコア結果を表示するテキスト
    [SerializeField]
    private GameObject Button;//リトライとタイトルボタンをまとめた親オブジェクト


    private float ResultStartTime = 0.0f;//結果を出すまでの時間

    [SerializeField]
    private float ResultEndTime;//この時間を超えると結果を出す
    private Candle candle;
    [SerializeField]
    private Fire fire;

    GameObject[] GameOverAssets;

    private Coroutine SampleCoroutine = null;
    // Start is called before the first frame update
    void Start()
    {


        GameOverAssets = new GameObject[] { GameOverText, ScoreParents, Button };
        foreach (GameObject asset in GameOverAssets)
        {
            asset.SetActive(false);//ゲームオーバー用のアセット非アクティブ化
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
        candle = fire.UseCandle();//使用しているキャンドルのスクリプトを使用
        GameState_test state = GetComponent<GameState_test>();

        if (candle.GetBurnOut())
        {
            state.JudgeGameOver = true;
        }
        if (state.JudgeGameOver)
        {

            GameOverCanvas.SetActive(true);
            if (SampleCoroutine == null)
            {
                //コルーチン開始
                SampleCoroutine = StartCoroutine(GameCoroutine(2.0f, GameOverAssets));
            }

            Background.SetActive(true);
            ScoreResult(ResultEndTime);//ResultEndTime時間ランダムにスコアを表示
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
        GameState_test state = GetComponent<GameState_test>();
        ResultStartTime += Time.deltaTime;
        if (ResultStartTime < Endtime)
        {
            int Assignscore = RandomScore(500, 1500);
            Score_Text.text = Assignscore.ToString();
        }
        else
        {
            int Assignscore = state.JudgecurrentScore;
            Score_Text.text = Assignscore.ToString();//テスト用
        }
    }


    public IEnumerator GameCoroutine(float delay, GameObject[] Asset)
    {
        GameState_test state = GetComponent<GameState_test>();
        while (i < Asset.Length)
        {
            //ゲームオーバアセット全て表示されるまで継続
            Asset[i].SetActive(true);
            i++;
            yield return new WaitForSeconds(delay);
        }
        //state.NotGameOver();
        SampleCoroutine = null;
    }

    public void PressRetry()//リトライボタンを押した時
    {
        /* GameState_test state = GetComponent<GameState_test>();


         Firepos.SetActive(true);
         Canldlecon.SetActive(true);
         PL.SetActive(true);
         PL.transform.localScale = SaveScale;
         PL.transform.position = SavePLpos;
         Foot.SetActive(true);
         Foot.transform.position = SaveFootpos;
         state.JudgeGameOver = false;
         Debug.Log("a");*/
        SceneManager.LoadScene("PrototypeScene");//今だけ
    }
}



