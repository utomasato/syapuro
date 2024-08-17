using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
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
    private GameObject Button;//リトライとタイトルボタンをまとめた親オブジェクト

    private Candle candle;
    [SerializeField]
    private Fire fire;

    GameObject[] GameOverAssets;

    private Coroutine SampleCoroutine = null;

    [SerializeField] GameState gameState;

    void Start()
    {
        GameOverAssets = new GameObject[] { GameOverText, ScoreParents, Button };
        foreach (GameObject asset in GameOverAssets)
        {
            asset.SetActive(false);//ゲームオーバー用のアセット非アクティブ化
        }
        Background.SetActive(false);
    }

    private int i = 0;

    public void GameOverSystem()
    {
        candle = fire.UseCandle();//使用しているキャンドルのスクリプトを使用

        GameOverCanvas.SetActive(true);
        if (SampleCoroutine == null)
        {
            //コルーチン開始
            SampleCoroutine = StartCoroutine(GameCoroutine(2.0f, GameOverAssets));
        }

        Background.SetActive(true);
    }

    /*public int RandomScore(int min, int max)
    {
        return Random.Range(min, max);
    }

    public void ScoreResult(float Endtime, TMP_Text ScoreText)
    {
        GameState_test state = GetComponent<GameState_test>();
        ResultStartTime += Time.deltaTime;
        if (ResultStartTime < Endtime)
        {
            int Assignscore = RandomScore(500, 1500);
            ScoreText.text = Assignscore.ToString();
        }
        else
        {
            if (state != null)
            {
                int Assignscore = state.GetScore();
                ScoreText.text = Assignscore.ToString();
            }
        }
    }
*/

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

        SampleCoroutine = null;
    }

    /*
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
         Debug.Log("a");
         * /
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    */
}



