using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class canLength : MonoBehaviour
{
    public float canLen = 0;//蝋燭の長さを表す数値
    public Status status;//通常モード(0)かブーストモード(1)か選択する変数
    private float time = 0;
    public float[] times = new float[2];//蝋燭が1メモリ減るのにかかる時間(status次第で変化)
    // Start is called before the first frame update
    void Start()
    {
        if (canLen == 0) canLen = this.gameObject.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= times[(int)status])
        {
            time = 0;
            changeLength(0.01F);
        }
    }

    void changeLength(float variation)//蝋燭の長さ、位置を設定するメソッド
    {
        if (canLen == 0) return;
        canLen -= variation;
        if (canLen < 0) canLen = 0;
        Vector3 temp = this.gameObject.transform.localScale;
        this.gameObject.transform.localScale = new Vector3(temp.x, canLen, temp.z);
        temp = this.gameObject.transform.localPosition;
        this.gameObject.transform.localPosition = new Vector3(temp.x, temp.y - variation, temp.z);
    }
}
public enum Status
{
    normal = 0,
    boost = 1

}
