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
            Vector3 ls = transform.localScale;
            ls.y = candle.GetSize() + legsize;
            transform.localScale = ls;
            candle.Moving_sub(transform.position + new Vector3(0f, legsize / 2f, 0f));
            Vector3 footpos = transform.position;
            footpos.y -= ls.y / 2 - legsize;
            foot.transform.position = footpos;
        }
        else
        {
            candle.Moving_sub(transform.position);
        }
    }

    public void Move(float x)
    {
        transform.position += new Vector3(x * Time.deltaTime, 0, 0);
    }

    public void Jump()
    {
        if (CanJump)
        {
            rb.velocity = Vector3.up * jumpPower;
            //CanJump = false;
        }
    }

    public void WakeUp_sub() // 憑依される 足が生える
    {
        IsBurning = true;
        Vector3 ls = transform.localScale;
        ls.y = candle.GetSize() + legsize;
        transform.localScale = ls;
        transform.position += new Vector3(0f, legsize / 2f, 0f);
        Vector3 p = transform.position;
        p.z = 0f;
        transform.position = p;
    }

    public void Sleep_sub() // 抜け殻になる 足が消える
    {
        Vector3 ls = transform.localScale;
        ls.y = candle.GetSize();
        transform.localScale = ls;
        if (IsBurning)
        {
            IsBurning = false;
            transform.position += new Vector3(0f, legsize / 2f, 0f);
        }
        Vector3 p = transform.position;
        p.z = 2f;
        transform.position = p;
        foot.transform.position = new Vector3(0f, -1000f, 0f);
    }
}
