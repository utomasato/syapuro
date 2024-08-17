using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEngine.SceneManagement;

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
    private GameObject Button;
    [SerializeField]
    private GameObject PLFire;//プレイヤー

    GameObject[] GameClearAssets;

    private Coroutine SampleCoroutine;

    // [SerializeField]
    // private GameObject Lamp;//仮
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

        InitializeLamp(state.GetMaxLamp());
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

    void InitializeLamp(int lampCount)
    {
        /*今だけ無理矢理修正しています*/
        GameObject lamp = GameObject.Find("lamp (1)");
        float ScreenHeight = Screen.height;
        float ScreenWidth = 30;

        float Space = ScreenWidth / (lampCount + 1); // +1で両端に余白を追加

        for (int i = 0; i < lampCount; i++)
        {
            GameObject LampInstance = Instantiate(lamp);
            LampInstance.transform.localScale *= 3f;

            // Lampのx座標を計算し、等間隔に配置
            float xPos = Space * (i + 1);

            // LampのTransformを設定
            LampInstance.transform.position = new Vector3(xPos - 13, 4, -17);
        }
    }
}
