using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameClear : MonoBehaviour
{
    [SerializeField]
    private GameObject ClearedCanvas;//ゲームクリアキャンバス
    [SerializeField]
    private GameObject Background;//Image
    [Tooltip("CLEARと書かれているテキストを挿入してください")]
    [SerializeField]
    private GameObject ClearText;//Clearと書かれているテキスト
    [SerializeField]
    private GameObject ScoreParents;//スコア表示の親オブジェクト
    [SerializeField]
    private TMP_Text Score_Text;//スコア結果を表示するテキスト
    [SerializeField]
    private GameObject Button;
    [SerializeField]
    private GameObject PLFire;//プレイヤー

    GameObject[] GameClearAssets;


    private Coroutine SampleCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        ClearedCanvas.SetActive(false);
        GameClearAssets = new GameObject[] { ClearText, ScoreParents, Button };
        foreach (GameObject asset in GameClearAssets)
        {
            asset.SetActive(false);//クリアキャンバス非アクティブ化
        }

    }

    // Update is called once per frame
    void Update()
    {

        ClearSystem();
    }


    void ClearSystem()
    {
        GameState state = GetComponent<GameState>();
        GameOver GO = GetComponent<GameOver>();

        if (state.JudgeGameClear)
        {
            PLFire.SetActive(false);
            state.JudgeRank(Score_Text);
            ClearedCanvas.SetActive(true);
            if (SampleCoroutine == null)
            {
                SampleCoroutine = StartCoroutine(GO.GameCoroutine(2.0f, GameClearAssets));
            }

        }
    }
}
