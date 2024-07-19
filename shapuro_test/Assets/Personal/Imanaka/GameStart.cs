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
    // Start is called before the first frame update
    void Start()
    {
        CountdownCandle.SetActive(false);
        //  StartCanvas.SetActive(false); コメント解除予定
        StartCanvas.SetActive(true);

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

    public void PushStart()//Startボタンプッシュ
    {
        GameState State = GetComponent<GameState>();
        State.JudgeCountdown = true;
    }
}
