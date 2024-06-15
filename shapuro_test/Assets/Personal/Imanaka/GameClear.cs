using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameClear : MonoBehaviour
{
    [SerializeField]
    private GameObject Background;
    [SerializeField]
    private GameObject ClearText;
    [SerializeField]
    private GameObject ScoreParents;//スコア表示の親オブジェクト
    [SerializeField]
    private TMP_Text Score_Text;//スコア結果を表示するテキスト
    [SerializeField]
    private GameObject Button;

    GameObject[] GameClearAssets;

    private bool IsClear = false;

    private Coroutine SampleCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        GameClearAssets = new GameObject[] { ClearText, ScoreParents, Button };
        foreach (GameObject asset in GameClearAssets)
        {
            asset.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Invoke("Test", 2.0f);
        ClearSystem();
    }
    void Test()
    {
        IsClear = true;
    }

    void ClearSystem()
    {
        GameOver GO = GetComponent<GameOver>();
        if (IsClear)
        {
            if (SampleCoroutine == null)
            {
                SampleCoroutine = StartCoroutine(GO.GameCoroutine(2.0f, GameClearAssets));
            }
        }
    }
}
