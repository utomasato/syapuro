using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameOver : MonoBehaviour
{

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

    void Test()
    {
        IsGameOver = true;
    }

    void GameOverSystem()
    {
        if (IsgameStart)
        {
            currentScale = PL.transform.localScale;
            /*  if (currentScale.y < 0)
              {
                  IsGameOver = true;
                  IsgameStart = false;
              }*/
        }
        if (IsGameOver)
        {
            if (SampleCoroutine == null)
            {
                Debug.Log("aaa");
                SampleCoroutine = StartCoroutine(GameCoroutine(2.0f, GameOverAssets));
            }

            Background.SetActive(true);
            ScoreResult(ResultEndTime);
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
        while (i < Asset.Length)
        {

            Asset[i].SetActive(true);
            i++;
            yield return new WaitForSeconds(delay);
        }
        IsGameOver = false;
        SampleCoroutine = null;
    }
}

