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
    private List<GameObject> FireImage = new List<GameObject>();

    private GameObject[] FireImages;
    // Start is called before the first frame update
    void Start()
    {
        int index = 0;
        CountdownCandle.SetActive(false);
        //  StartCanvas.SetActive(false); コメント解除予定
        StartCanvas.SetActive(true);
        foreach (GameObject obj in FireImage)
        {
            FireImage[index] = obj;
            index++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        GameState State = GetComponent<GameState>();
        if (State.JudgeCountdown)
        {
            NotStartCanvas.SetActive(false);
            CountdownCandle.SetActive(true);
        }
    }
    private int i = 0;
    IEnumerator CountdownCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
    public void PushStart()//Startボタンプッシュ
    {
        GameState State = GetComponent<GameState>();
        State.JudgeCountdown = true;
    }
}
