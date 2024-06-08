using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle_test : MonoBehaviour
{
    [SerializeField] private float life = 2f;       // 全体量
    [SerializeField] private float SpL;        // lifeと見た目の大きさの割合 同じ長さでも持つ時間が違うかも
    [SerializeField] private GameObject firePoint;  // 火がつくところ
    [SerializeField] private CandleCon_test candlecon;
    private float size0;
    private float size;
    private bool IsBurning = false;
    private Vector3 hedPos0;
    private Vector3 hedPos;

    void Start()
    {

        size0 = transform.lossyScale.y;
        size = size0;
        SpL = size0 / life;
        hedPos0 = firePoint.transform.position - this.transform.position;
        Sleep();
    }

    void Update()
    {
        if (!IsBurning) return;
        hedPos = transform.position + hedPos0 * life * SpL / size0;
        firePoint.transform.position = hedPos;
    }

    public void Shorten(float speed) // 蝋燭を短くする
    {
        life -= speed * Time.deltaTime;
        size = life * SpL;
        Vector3 ls = transform.localScale;
        ls.y = size;
        transform.localScale = ls;
        if (life <= 0f)
        {
            Destroy(candlecon.gameObject);
            Destroy(firePoint);
            Destroy(this.gameObject);
        }
    }

    public void Moving_sub(Vector3 pos)
    {
        transform.position = pos;
    }

    public void WakeUp() // 憑依される
    {
        IsBurning = true;
        Vector3 p = transform.position;
        p.z = 0f;
        transform.position = p;
        candlecon.WakeUp_sub();
    }

    public void Sleep() // 抜け殻になる
    {
        IsBurning = false;
        Vector3 p = transform.position;
        p.z = 2f;
        transform.position = p;
        candlecon.Sleep_sub();
    }

    public Vector3 GetHedPos() // 炎の位置
    {
        return hedPos;
    }

    public float GetSize() // 蝋燭のサイズ
    {
        return size;
    }
}