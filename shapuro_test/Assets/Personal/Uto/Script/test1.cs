// Candle.csの動作確認用
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    public Candle candle;
    //float speed = 1.0f;
    //bool a = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxis("JoyHorizontal"));
        if (Input.anyKeyDown)
        {
            Debug.Log("Pressed any key");
        }
    }
}
