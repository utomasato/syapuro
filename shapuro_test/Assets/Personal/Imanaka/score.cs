using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class score : MonoBehaviour
{
    [SerializeField]
    TMP_Text Score;

    private GameManager gameManager;//SetGameスクリプトのGameManagerクラスを使用

    int Result;

    int Random_Score;

    float StartRandomScore = 0f;//スコア結果が出る前に、ランダムにスコアを表示し続けるための時間

    [SerializeField]
    public float FinishRandom;//ランダムスコア終了
    // Start is called before the first frame update
    void Start()
    {
        gameManager = new GameManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetisGameOver())
        {
            StartRandomScore += Time.deltaTime;
            if (StartRandomScore <= FinishRandom)
            {
                Random_Score = RandomScore(500, 1500);
                Score.text = Random_Score.ToString();
            }
            else
            {
                Score.text = "200";
            }
        }
    }

    int RandomScore(int min, int max)
    {
        return Random.Range(min, max);
    }
}
