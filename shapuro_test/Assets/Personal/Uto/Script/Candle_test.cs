using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle_test : MonoBehaviour
{
    [SerializeField] private float life = 2f;       // 全体量
    [SerializeField] private float SpL;        // lifeと見た目の大きさの割合 同じ長さでも持つ時間が違うかも
    [SerializeField] private float jumpPower = 8f;
    [SerializeField] private GameObject firePoint;  // 火がつくところ
    private float size0;
    private bool CanJump = true;
    private bool IsBurning = false;
    private Vector3 hedPos0;
    private Vector3 hedPos;
    private Rigidbody rb;

    void Start()
    {
        size0 = transform.lossyScale.y;
        SpL = size0 / life;
        rb = GetComponent<Rigidbody>();
        hedPos0 = firePoint.transform.position - this.transform.position;
        if (IsBurning)
        {
            Vector3 p = transform.position;
            p.z = 0f;
            transform.position = p;
        }
        else
        {
            Vector3 p = transform.position;
            p.z = 2f;
            transform.position = p;
        }
    }

    void Update()
    {
        if (!IsBurning) return;
        hedPos = transform.position + hedPos0 * life * SpL / size0;
        firePoint.transform.position = hedPos;
    }

    public void Shorten(float speed)
    {
        life -= speed * Time.deltaTime;
        transform.localScale = new Vector3(1, life * SpL, 1);
        if (life <= 0f)
        {
            Destroy(firePoint);
            Destroy(this.gameObject);
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
            CanJump = false;
        }
    }

    public void WakeUp()
    {
        GetComponent<Rigidbody>().velocity = Vector3.up * jumpPower / 2f;
        CanJump = false;
        IsBurning = true;
        Vector3 p = transform.position;
        p.z = 0f;
        transform.position = p;
    }

    public void Sleep()
    {
        IsBurning = false;
        Vector3 p = transform.position;
        p.z = 2f;
        transform.position = p;
    }

    public Vector3 GetHedPos()
    {
        return hedPos;
    }

    void OnCollisionEnter(Collision collision)
    {
        CanJump = true; // 地面に接触したらフラグを立てる
    }
}