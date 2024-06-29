// Candle.csの動作確認用
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    public Candle candle;
    float speed = 1.0f;
    bool a = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            candle.Move(-speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            candle.Move(speed);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            candle.Shorten(0.1f);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            candle.Jump();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!a) candle.WakeUp();
            else candle.Sleep();
            a = !a;
        }
    }
}
