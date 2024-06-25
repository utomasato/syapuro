using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireWall : MonoBehaviour
{
    public float time = 0;
    public bool FireStart = false;
    void FixedUpdate()
    {
        if (FireStart == true)
        {
            time += Time.deltaTime;
            if (time >= 3)
            {
                //オブジェクトを消すかどうにかするプログラムをここに書く
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && FireStart == false)
        {
            time += Time.deltaTime;
            if (time >= 1)
            {
                FireStart = true;
                //燃えてるエフェクトをここにつける
            }
        }
    }
}
