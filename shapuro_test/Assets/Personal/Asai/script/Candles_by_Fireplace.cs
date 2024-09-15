using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Candles_by_Fireplace : MonoBehaviour
{
    public GameObject parent;
    public GameObject createObject;
    public int MaxLampCount = 0;
    public int LampCount = 0;
    public float radius = 1.5f;
    public float repeat = 1f;
    //public List<bool> BurningLampList;
    public List<GameObject> dinplayLampList;
    void Start()
    {
        // BurningLampList = new List<bool>();
        // for (int i = 0; i < MaxLampCount; i++)
        // {
        //     BurningLampList.Add(false);
        // }
        create();
    }
    public void MiniCandlefire()
    {
        dinplayLampList[LampCount].GetComponent<minicandle>().minicandlefire();
        LampCount++;
    }
    public void create()
    {
        var oneCycle = 2.0f * Mathf.PI; // sin の周期は 2π

        for (var i = 0; i < MaxLampCount; ++i)
        {

            var point = ((float)i / (MaxLampCount - 1.0f)) * oneCycle; // 周期の位置 (1.0 = 100% の時 2π となる)
            var repeatPoint = point * repeat; // 繰り返し位置

            var x = parent.transform.position.x + Mathf.Cos(repeatPoint) * radius;
            var y = parent.transform.position.y + Mathf.Sin(repeatPoint) * radius - 0.5f;

            var position = new Vector3(x, y);

            GameObject LampInstance = Instantiate(
                createObject,
                position,
                Quaternion.identity,
                transform
            );
            dinplayLampList.Add(LampInstance);
        }
    }
}
