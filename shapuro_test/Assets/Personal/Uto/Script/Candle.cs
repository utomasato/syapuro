using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    [SerializeField] private float startLife; // 初期life
    private float life; // 現在のライフ
    private float startSize; // 初期サイズ
    private float size; // 現在のサイズ
    [SerializeField] private float minJumpPower, maxJumpPower;
    [SerializeField] private GameObject hed;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject foot;
    [SerializeField] private float footSize; // 足が生えた時にどれだけ背が高くなるか

    private bool IsBurning = false;
    private bool CanJump;
    private bool IsBurnOut = false;
    private Rigidbody rb;

    void Start()
    {
        life = startLife;
        startSize = transform.lossyScale.y;
        size = startSize;

        rb = GetComponent<Rigidbody>();
        foot.transform.position = new Vector3(0.0f, -100.0f, 0.0f); // 画面外に持っていく
    }

    void Update()
    {
        body.transform.position = transform.position;
        if (IsBurning)
        {
            body.transform.position += new Vector3(0.0f, footSize / 2.0f, 0.0f);
            foot.transform.position = body.transform.position - new Vector3(0.0f, size / 2.0f, 0.0f);
        }
        hed.transform.position = body.transform.position + new Vector3(0.0f, size / 2.0f, 0.0f);

    }

    public void Shorten(float burningSpeed) // 蝋燭を短くする
    {
        life -= burningSpeed * Time.deltaTime;
        size = startSize * (life / startLife);
        if (life <= 0.0f)
        {
            if (!IsBurnOut) BurnOut();
            return;
        }
        Vector3 ls = transform.localScale;
        ls.y = size;
        body.transform.localScale = ls;
        if (IsBurning) ls.y += footSize;
        transform.localScale = ls;
    }

    public void Move(float x) // 動かす
    {
        transform.position += new Vector3(x * Time.deltaTime, 0, 0);
    }

    public void Jump()
    {
        float jumpPower = Mathf.Lerp(minJumpPower, maxJumpPower, 1.0f - life / startLife);
        if (CanJump)
        {
            rb.velocity = Vector3.up * jumpPower;
            CanJump = false;
        }
    }

    public void WakeUp() // 足が生える
    {
        IsBurning = true;
        // 足が生える分位置が高くなる
        Vector3 pos = transform.position;
        pos.y += footSize / 2;
        transform.position = pos;
        // 足が生える分当たり判定が大きくなる
        Vector3 ls = transform.localScale;
        ls.y = size + footSize;
        transform.localScale = ls;
        //Debug.Log(this.name + " : WakeUp");
    }

    public void Sleep() // 足が消える
    {
        IsBurning = false;

        Vector3 pos = transform.position;
        pos.y += footSize / 2;
        transform.position = pos;

        Vector3 ls = transform.localScale;
        ls.y = size;
        transform.localScale = ls;
        foot.transform.position = new Vector3(0.0f, -100.0f, 0.0f); // 画面外に持っていく
        //Debug.Log(this.name + " : Sleep");
    }

    public void BurnOut()
    {
        Debug.Log(this.name + " : BurnOut");
        IsBurnOut = true;
        transform.parent.gameObject.SetActive(false);
    }

    public float GetSize()
    {
        return size;
    }

    void OnCollisionEnter(Collision other)
    {
        CanJump = true;
    }

    void OnCollisionExit(Collision other)
    {
        CanJump = false;
    }
}
