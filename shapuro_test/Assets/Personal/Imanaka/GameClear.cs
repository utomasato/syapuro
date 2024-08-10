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

    void Start()
    {
        ClearedCanvas.SetActive(false);
        GameClearAssets = new GameObject[] { ClearText, ScoreParents, Button };
        foreach (GameObject asset in GameClearAssets)
        {
            asset.SetActive(false);//クリアキャンバス非アクティブ化
        }

    }


    public void ClearSystem()
    {
        GameState state = GetComponent<GameState>();
        //GameOver GO = GetComponent<GameOver>();

        PLFire.SetActive(false);
        state.JudgeRank(Score_Text);
        ClearedCanvas.SetActive(true);
        if (SampleCoroutine == null)
        {
            SampleCoroutine = StartCoroutine(GameCoroutine(2.0f, GameClearAssets));
        }
    }

    public IEnumerator GameCoroutine(float delay, GameObject[] Asset)
    {
        int i = 0;
        GameState_test state = GetComponent<GameState_test>();
        while (i < Asset.Length)
        {
            //ゲームクリアアセット全て表示されるまで継続
            Asset[i].SetActive(true);
            i++;
            yield return new WaitForSeconds(delay);
        }

        SampleCoroutine = null;
    }
}
