using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    [SerializeField]
    private GameObject StartCanvas;
    [SerializeField]
    private GameObject NotStartCanvas;//Startボタン押す前に表示されるキャンバス
    [SerializeField]
    private GameObject CountdownCandle;//Startボタン押した後に表示されるキャンバス
    [SerializeField]
    private List<GameObject> FireImage = new List<GameObject>(); //カウントダウン時の炎

    private GameObject[] FireImages;

    private Coroutine CountCoroutine;
    [SerializeField]
    private GameObject PL;

    void Start()
    {

        int index = 0;
        //  StartCanvas.SetActive(false); コメント解除予定
        FireImages = new GameObject[FireImage.Count];
        //  StartCanvas.SetActive(true);
        foreach (GameObject obj in FireImage)
        {
            FireImages[index] = obj;
            FireImages[index].SetActive(false);
            index++;
        }
        CountdownCandle.SetActive(false);

    }

    void Update()
    {
        GameState State = GetComponent<GameState>();
        if (/*State.JudgeCountdown &&*/ CountCoroutine == null)
        {

            NotStartCanvas.SetActive(false);
            CountdownCandle.SetActive(true);
            CountCoroutine = StartCoroutine(CountdownCoroutine(1.0f));
        }
    }

    private int i = 0;
    IEnumerator CountdownCoroutine(float delay)
    {
        GameState State = GetComponent<GameState>();
        while (i < FireImages.Length)
        {
            FireImages[i].SetActive(true);
            i++;
            yield return new WaitForSeconds(delay);
        }
        StartCanvas.SetActive(false);
        //State.SetGameStart();
        CountCoroutine = null;
    }

    public void PushStart()//Startボタンプッシュ
    {
        GameState State = GetComponent<GameState>();
        //State.JudgeCountdown = true;
    }

}
