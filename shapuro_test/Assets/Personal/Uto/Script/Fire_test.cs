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
    private float t;
    [SerializeField] private Candle_test candle;
    //private bool IsBigFire = false;
    private bool IsOnCandle = true;
    private bool IsGameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        movingSpeed = movingSpeed1;
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
            transform.position = candle.GetHedPos();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.localScale = new Vector3(1, 2, 1);
                //IsBigFire = true;
                movingSpeed = movingSpeed2;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                transform.localScale = new Vector3(1, 1, 1);
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
                candle.Move(-movingSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                candle.Move(movingSpeed);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                candle.Jump();
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

    public void fly()
    {
        if (IsOnCandle)
        {
            transform.localScale = new Vector3(1, 1, 1);
            //IsBigFire=false;
            transform.position += new Vector3(0, 1f, 0);
            IsOnCandle = false;
            t = 0f;
        }

    }

    public bool GetIsOnCandle()
    {
        return IsOnCandle;
    }

    public void Transfer(Candle_test NewCandle)
    {
        candle = NewCandle;
        IsOnCandle = true;
        candle.Jump();
    }

}
