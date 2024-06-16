using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asai_Jumptest : MonoBehaviour
{
    [SerializeField] private CandleCon_test candlecon;
    [SerializeField] private Candle_test candle;
    [SerializeField] private Fire_test parent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if (1.5 < candle.GetSize)
        // {
        //     CandleCon_test.jumpPower = 8.0f;
        // }
        // else if (1.0 < candle.GetSize)
        // {
        //     CandleCon_test.jumpPower = 10.0f;
        // }
        // else if (0.5 < candle.GetSize)
        // {
        //     CandleCon_test.jumpPower = 13.0f;
        // }
        // else if (0 < candle.GetSize)
        // {
        //     CandleCon_test.jumpPower = 16.0f;
        // }
    }
    public void Test(FirePosition_test NewCandle)
    {
        candle = NewCandle.candle;
        candlecon = NewCandle.candlecon;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!parent.GetIsOnCandle() && other.gameObject.CompareTag("FirePosition")) // 火の玉状態で蝋燭に当たったら
        {
            Test(other.gameObject.GetComponent<FirePosition_test>());
        }
    }
}
