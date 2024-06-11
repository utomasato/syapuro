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
    [SerializeField] private Candle_test candle;
    [SerializeField] private CandleCon_test candlecon;
    //private bool IsBigFire = false;
    private bool IsOnCandle = false;
    private bool IsGameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        movingSpeed = movingSpeed1;
        //candle.WakeUp();
        //IsOnCandle = true;
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

        if (IsOnCandle)
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
        else
        {
            t += Time.deltaTime;
            if (t >= flingtime)
            {
                if (candle == null)
                {
                    Debug.Log("GameOver");
                    IsGameOver = true;
                }
                else
                {
                    IsOnCandle = true;
                    candle.WakeUp();
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
        candle = NewCandle.candle;
        candlecon = NewCandle.candlecon;
        IsOnCandle = true;
        candle.WakeUp();
    }

    public bool GetIsOnCandle()
    {
        return IsOnCandle;
    }


}
