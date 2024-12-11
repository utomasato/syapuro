using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private TMP_Text RankText;
    [SerializeField]
    private GameObject Button;

    [SerializeField]
    private GameObject PLFire;//プレイヤー

    GameObject[] GameClearAssets;

    [SerializeField] private GameObject lamp;
    [SerializeField] private float minInterval, maxInterval;
    private List<GameObject> dinplayLampList;

    [SerializeField] private GameState gameState;
    [SerializeField] private UnityEngine.UI.Button button;

    private Coroutine SampleCoroutine;

    // [SerializeField]
    // private GameObject Lamp;//仮
    void Start()
    {
        ClearedCanvas.SetActive(true);

        ClearedCanvas.SetActive(false);
        GameClearAssets = new GameObject[] { ClearText, ScoreParents, Button };
        foreach (GameObject asset in GameClearAssets)
        {
            asset.SetActive(false);//クリアキャンバス非アクティブ化
        }
        dinplayLampList = new List<GameObject>();
        //RankText.enabled = false;
    }


    public void ClearSystem()
    {
        GameState state = GetComponent<GameState>();

        InitializeLamp(state.GetMaxLamp());
        ClearedCanvas.SetActive(true);
        if (RankText != null)
        {
            RankText.text = state.Rank(RankText);
        }
        if (SampleCoroutine == null)
        {
            SampleCoroutine = StartCoroutine(GameCoroutine(1.0f, GameClearAssets));
        }
    }

    public IEnumerator GameCoroutine(float delay, GameObject[] Asset)
    {
        int i = 0;
        //GameState_test state = GetComponent<GameState_test>();
        while (i < Asset.Length)
        {
            //ゲームクリアアセット全て表示されるまで継続
            Asset[i].SetActive(true);

            if (i == 1)//Asset[i]がScoreParentsなら
            {
                for (int j = 0; j < dinplayLampList.Count; j++)
                {
                    if (gameState.GetBurningLampList[j])
                    {
                        dinplayLampList[j].GetComponent<DisplayLamp>().Ignition(gameState);
                        yield return new WaitForSeconds(0.5f);
                    }
                }
                //RankText.enabled = true;
            }

            i++;
            if (i < Asset.Length)
                yield return new WaitForSeconds(delay);
        }
        gameState.SetButton(button);

        SampleCoroutine = null;
    }

    void InitializeLamp(int lampCount)
    {

        for (int i = 0; i < lampCount; i++)
        {
            GameObject LampInstance = Instantiate(lamp, ScoreParents.transform);
            RectTransform lampTransform = LampInstance.GetComponent<RectTransform>();
            Vector3 pos = lampTransform.anchoredPosition;
            float interval = Mathf.Lerp(minInterval, maxInterval, 1f - lampCount / 2f);
            pos.x = -(lampCount + 1) * interval / 2 + i * interval;
            pos.y = 30f;
            lampTransform.anchoredPosition = pos;

            foreach (RectTransform child in LampInstance.GetComponentsInChildren<RectTransform>())
            {
                child.sizeDelta *= 0.12f;
                if (child != lampTransform)
                    child.anchoredPosition = new Vector3(-9.6f, 86.4f, 0f);
            }

            dinplayLampList.Add(LampInstance);
        }
    }
}
