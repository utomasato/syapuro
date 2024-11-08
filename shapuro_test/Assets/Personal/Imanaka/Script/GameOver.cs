using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private GameObject Cause;
    [SerializeField]
    private GameObject ScoreParents;//スコア表示の親オブジェクト
    [SerializeField]
    private GameObject Button;//リトライとタイトルボタンをまとめた親オブジェクト

    private Candle candle;
    [SerializeField]
    private Fire fire;
    private string DeathCause = "";

    GameObject[] GameOverAssets;

    private Coroutine SampleCoroutine = null;

    [SerializeField] GameState gameState;
    [SerializeField] private UnityEngine.UI.Button button;


    void Start()
    {
        Cause = GameOverCanvas.transform.Find("Cause").gameObject;
        GameOverAssets = new GameObject[] { GameOverText, Cause, ScoreParents, Button };
        foreach (GameObject asset in GameOverAssets)
        {
            asset.SetActive(false);//ゲームオーバー用のアセット非アクティブ化
        }
        Background.SetActive(false);
    }

    private int i = 0;

    public void GameOverSystem()
    {
        candle = fire.GetCandle();//使用しているキャンドルのスクリプトを使用


        GameOverCanvas.SetActive(true);
        if (SampleCoroutine == null)
        {
            //コルーチン開始
            SampleCoroutine = StartCoroutine(GameCoroutine(0.2f, GameOverAssets));
        }

        Background.SetActive(true);
    }



    public IEnumerator GameCoroutine(float delay, GameObject[] Asset)
    {
        Cause.GetComponent<TextMeshProUGUI>().text = DeathCause;
        GameState_test state = GetComponent<GameState_test>();
        while (i < Asset.Length)
        {
            //ゲームオーバアセット全て表示されるまで継続
            Asset[i].SetActive(true);
            i++;
            if (i < Asset.Length)
                yield return new WaitForSeconds(delay);
        }
        gameState.SetButton(button);

        SampleCoroutine = null;
    }
    public void SetDeathCause(string Cause)
    {
        DeathCause = Cause;
    }

}



