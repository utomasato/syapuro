using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCandle_test : MonoBehaviour
{
    [SerializeField]
    List<int> StageList = new List<int>();//ステージごとの火をつける蝋燭のオブジェクトを保存

    private int[] Stages;//動的配列のデータを配列へ


    private int i = 0;//配列の要素数を指定。0はチュートリアル　1からステージレベル

    private int Currentlevel = 0;//現在のレベル
    // Start is called before the first frame update
    void Start()
    {
        /*   foreach (GameObject obj in StageList)
           {
               Stages[i] = obj;
               i++;
           }*/

        for (int i = 0; i < StageList.Count; i++)
        {
            Stages[i] = StageList[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameState_test state = GetComponent<GameState_test>();

    }

    void JudgeCurrentLight()//現在のつけたライトの個数を確認
    {

    }

}
