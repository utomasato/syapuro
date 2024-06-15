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

    private Coroutine SampleCoroutine;
    // Start is called before the first frame update
    void Start()
    {

        GameOverAssets = new GameObject[] { GameOverText, Score_Text.gameObject, Button };
        foreach (GameObject asset in GameOverAssets)
        {
            asset.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("Test", 2.0f);
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
            SampleCoroutine = StartCoroutine(GameOverCoroutine(2.0f));
            Background.SetActive(true);
            ScoreResult(ResultEndTime);
        }
    }

    int RandomScore(int min, int max)
    {
        return Random.Range(min, max);
    }

    void ScoreResult(float Endtime)
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

    private int i = 0;
    IEnumerator GameOverCoroutine(float delay)
    {
        while (i < GameOverAssets.Length)
        {
            GameOverAssets[i].SetActive(true);
            yield return new WaitForSeconds(delay);
            i++;
        }
        SampleCoroutine = null;
    }
}

