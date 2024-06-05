using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle_test : MonoBehaviour
{
    [SerializeField] private float life = 2f;       // 全体量
    [SerializeField] private float SpL;        // lifeと見た目の大きさの割合 同じ長さでも持つ時間が違うかも
    [SerializeField] private float jumpPower = 8f;
    [SerializeField] private GameObject firePoint;  // 火がつくところ
    private bool CanJump = true;
    private Vector3 hedPos0;
    private Vector3 hedPos;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        SpL = transform.lossyScale.y / life;
        rb = GetComponent<Rigidbody>();
        hedPos0 = firePoint.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        hedPos = transform.position + hedPos0 * life * SpL / 2;
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

    public Vector3 GetHedPos()
    {
        return hedPos;
    }

    void OnCollisionEnter(Collision collision)
    {
        CanJump = true; // 地面に接触したらフラグを立てる
    }
}