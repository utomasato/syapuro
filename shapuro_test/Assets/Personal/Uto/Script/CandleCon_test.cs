/*
    蝋燭の動きと当たり判定担当
    足が生えてる分蝋燭の大きさと動いてる時の当たり判定に差があるため蝋燭の大きさを変える部分と蝋燭の動きの部分を分けた
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleCon_test : MonoBehaviour
{
    [SerializeField] private Candle_test candle;
    [SerializeField] private float legsize = 0.6f;
    [SerializeField] private float jumpPower = 8f;
    [SerializeField] private GameObject foot;
    private bool CanJump = true;
    private bool IsBurning = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBurning)
        {
            Vector3 ls = transform.localScale; // candleが縮むのに合わせて小さくなる
            ls.y = candle.GetSize() + legsize;
            transform.localScale = ls;
            candle.Moving_sub(transform.position + new Vector3(0f, legsize / 2f, 0f)); // candleの位置を合わせる
            Vector3 footpos = transform.position;
            footpos.y -= ls.y / 2 - legsize;
            foot.transform.position = footpos; // 足の位置を合わせる
        }
        else
        {
            candle.Moving_sub(transform.position); // candleの位置を合わせる
        }
    }

    public void Move(float x) // 動かす
    {
        transform.position += new Vector3(x * Time.deltaTime, 0, 0);
    }

    public void Jump()
    {
        if (1.5f <= candle.GetSize())
        {
            jumpPower = 8.0f;
        }
        else if (1.0f <= candle.GetSize())
        {
            jumpPower = 9.0f;
        }
        else if (0.5f <= candle.GetSize())
        {
            jumpPower = 10.0f;
        }
        else if (0f <= candle.GetSize())
        {
            jumpPower = 11.0f;
        }
        if (CanJump)
        {
            rb.velocity = Vector3.up * jumpPower;
            CanJump = false;
        }
    }

    public void WakeUp_sub() // 憑依される 足が生える
    {
        IsBurning = true;
        Vector3 ls = transform.localScale;
        ls.y = candle.GetSize() + legsize;
        transform.localScale = ls; // 足が生える分背が高くなる
        transform.position += new Vector3(0f, legsize / 2f, 0f); // 足が生える分位置が高くなる
        Vector3 p = transform.position;
        p.z = 0f;
        transform.position = p; // レイヤーを変える
    }

    public void Sleep_sub() // 抜け殻になる 足が消える
    {
        Vector3 ls = transform.localScale;
        ls.y = candle.GetSize();
        transform.localScale = ls; // 足がなくなる分小さくなる
        if (IsBurning)
        {
            IsBurning = false;
            transform.position += new Vector3(0f, legsize / 2f, 0f);
        }
        Vector3 p = transform.position;
        p.z = 2f;
        transform.position = p; // レイヤーを変える
        //foot.transform.position = new Vector3(0f, -1000f, 0f); // 足をどっかやる
    }

    void OnCollisionEnter(Collision other)
    {
        CanJump = true;
    }

    void OnCollisionExit(Collision other)
    {
        CanJump = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsBurning && other.gameObject.CompareTag("Goal")) // 憑依状態でゴールしたら
        {
            Debug.Log("CLEAR");
            GameObject.Find("GameState").GetComponent<GameState_test>().Clear();
        }
    }
}
