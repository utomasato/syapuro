using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireLocate : MonoBehaviour
{
    public GameObject player;
    public float absDis = 0;//製作陣が自由に決められる火と蝋燭の間隔を表す数値
    private float objDis;//火のオブジェクトの大きさによって変化する火と蝋燭の間隔を表す数値
    private float canDis;//蝋燭の長さによって変化する火と蝋燭の間隔を表す数値
    // Start is called before the first frame update
    private Vector3 fireLoc;
    void Start()
    {
        objDis = this.gameObject.transform.localScale.y / 2;
        fireLoc = player.transform.localPosition;
        fireLoc.y += canDis + objDis + absDis;
        canDis = player.transform.localScale.y;
        this.gameObject.transform.localPosition = fireLoc;//蝋燭との距離を設定
    }

    // Update is called once per frame
    void Update()
    {
        fireLoc = player.transform.localPosition;
        fireLoc.y += canDis + objDis + absDis;
        canDis = player.transform.localScale.y;
        this.gameObject.transform.localPosition = fireLoc;//蝋燭との距離を設定
    }
}
