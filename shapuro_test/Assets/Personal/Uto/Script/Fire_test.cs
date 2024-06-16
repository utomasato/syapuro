/*
炎の動き部分
炎から蝋燭に短くなる、動く、ジャンプの命令を出す
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_test : MonoBehaviour
{
    private float movingSpeed;
    [SerializeField] private float movingSpeed1, movingSpeed2;//移動速度 歩き、ダッシュ
    [SerializeField] private float BurningRate1, BurningRate2;//蝋の燃焼速度　弱火、強火
    [SerializeField] private float fireSpeed; // 火の玉状態の移動速度
    [SerializeField] private float flingtime; // 火の玉状態での生存時間
    private float t; // 火の玉状態になってからの経過時間
    [SerializeField] public Candle_test candle;
    [SerializeField] public CandleCon_test candlecon;
    //private bool IsBigFire = false;
    [SerializeField] private bool IsOnCandle = false;
    [SerializeField]
    private GameState_test state;
    private bool IsGameOver = false;
    private Vector3 startSize;


    // Start is called before the first frame update
    void Start()
    {
        movingSpeed = movingSpeed1;
        startSize = transform.localScale;
        //candle.WakeUp();
        if (IsOnCandle == true)
        {
            Transfer(null);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (candle == null)
        {
            fly();
        }
        if (IsGameOver)
        {
            return;
        }
        if (!state.JudgeGameOver)
        {
            if (IsOnCandle) // 蝋燭に着いているとき
            {
                //Debug.Log(candle.GetHedPos());
                transform.position = candle.GetHedPos();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Vector3 s = transform.localScale;
                    s.y = 2f;
                    transform.localScale = s;
                    //IsBigFire = true;
                    movingSpeed = movingSpeed2;
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    Vector3 s = transform.localScale;
                    s.y = 1f;
                    transform.localScale = s;
                    //IsBigFire = false;
                    movingSpeed = movingSpeed1;
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    candle.Shorten(BurningRate2);
                }
                else
                {
                    candle.Shorten(BurningRate1);
                }

                if (Input.GetKey(KeyCode.A))
                {
                    candlecon.Move(-movingSpeed);
                }

                if (Input.GetKey(KeyCode.D))
                {
                    candlecon.Move(movingSpeed);
                }

                if (Input.GetKeyDown(KeyCode.W))
                {
                    candlecon.Jump();
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    fly();
                }
            }
            else // 飛んでいる時
            {
                t += Time.deltaTime;
                transform.localScale = startSize * (1.0f - t / flingtime); // 火が小さくなっていく(火の玉状態での時間制限の可視化)
                if (t >= flingtime) // 火の玉になってから一定時間過ぎた時
                {
                    if (candle == null) // 直前に着いてた蝋燭がなくなっていれば
                    {
                        Debug.Log("GameOver");
                        IsGameOver = true;
                        GameObject.Find("GameState").GetComponent<GameState_test>().GameOver();
                    }
                    else // 直前に着いていた蝋燭が残っていれば
                    {
                        IsOnCandle = true; // 元いたところに戻る
                        candle.WakeUp();
                        transform.localScale = startSize;
                    }
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position += new Vector3(-fireSpeed * Time.deltaTime, 0, 0);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.position += new Vector3(fireSpeed * Time.deltaTime, 0, 0);
                }
                if (Input.GetKey(KeyCode.W))
                {
                    transform.position += new Vector3(0, fireSpeed * Time.deltaTime, 0);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    transform.position += new Vector3(0, -fireSpeed * Time.deltaTime, 0);
                }
            }
        }
    }

    public void fly()//蝋燭から離れる
    {
        if (IsOnCandle)
        {
            transform.localScale = new Vector3(1, 1, 1);
            //IsBigFire=false;
            transform.position += new Vector3(0, 0.6f, 0);
            IsOnCandle = false;
            t = 0f;
            if (candle != null)
            {
                candle.Sleep();
            }
        }
    }

    public void Transfer(FirePosition_test NewCandle)//蝋燭に憑依
    {
        if (NewCandle != null)
        {
            candle = NewCandle.candle;
            candlecon = NewCandle.candlecon;
        }
        IsOnCandle = true;
        candle.WakeUp();
        transform.localScale = startSize;
    }

    public bool GetIsOnCandle()
    {
        return IsOnCandle;
    }


}
