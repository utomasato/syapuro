/*
蝋燭が短くなる部分
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle_test : MonoBehaviour
{
    [SerializeField] public float life = 2f;       // 全体量
    [SerializeField] private float SpL;        // lifeと見た目の大きさの割合 同じ長さでも持つ時間が違うかも
    [SerializeField] private GameObject firePoint;  // 火がつくところ
    [SerializeField] private CandleCon_test candlecon; // 蝋燭の動きを司る
    private float size0; // 初期サイズ
    [SerializeField] public float size; // 現在のサイズ
    //private bool IsBurning = false;
    private Vector3 hedPos0; // 火がつく部分の初期位置
    private Vector3 hedPos; // 火がつく部分の位置

    private float SaveMaxLife;//全大量の初期値を保存

    void Start()
    {
        SaveMaxLife = life;
        size0 = transform.lossyScale.y;
        size = size0;
        SpL = size0 / life;
        hedPos0 = firePoint.transform.position - this.transform.position;
        Sleep();
    }

    void Update()
    {
        //if (!IsBurning) return;
        hedPos = transform.position + hedPos0 * life * SpL / size0; // 頭の位置を補正続ける(子オブジェクトにするとサイズも変わってしまうので動的に動かす)
        Vector3 h = hedPos;
        h.z -= 1.5f;
        firePoint.transform.position = h;
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
            Sleep();
            candlecon.gameObject.SetActive(false);
            firePoint.SetActive(false);
            this.gameObject.SetActive(false);
            life = SaveMaxLife;
        }
    }

    public void Moving_sub(Vector3 pos) // 自身の位置を補正する
    {
        transform.position = pos;
    }

    public void WakeUp() // 憑依される
    {
        //IsBurning = true;
        Vector3 p = transform.position;
        p.z = 0f; // レイヤーを変える
        transform.position = p;
        candlecon.WakeUp_sub(); // 動き側にも命令する
    }

    public void Sleep() // 抜け殻になる
    {
        //IsBurning = false;
        Vector3 p = transform.position;
        p.z = 2f; // レイヤーを変える
        transform.position = p;
        candlecon.Sleep_sub(); // 動き側にも命令する
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