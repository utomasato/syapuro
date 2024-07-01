using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIRE1 : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed;
    [SerializeField]
    private float BurnSpeed1;
    [SerializeField]
    private float BurnSpeed2;
    [SerializeField]
    private float Firespeed;//火の玉状態のスピード
    [SerializeField]
    private float SurviveTime;//火の玉状態の生存期間

    private float Firetime;//火の玉状態になってからの生存期間
    [SerializeField]
    private bool IsCandle;//ロウソクに炎がついているか


    [SerializeField]
    private Candle CandleScript;

    /*[SerializeField]
    private CandleCon_test CandleConScript;*/

    private Vector3 StartScale;
    [SerializeField]
    private GameObject Body;
    [SerializeField]
    private GameObject MainCandle;

    private bool IsChangeCandle = false;

    private GameObject TargetObject;
    // Start is called before the first frame update
    void Start()
    {
        StartScale = transform.localScale;
        RecoveryFire();
    }

    // Update is called once per frame
    void Update()
    {
        if (CandleScript == null)
        {
            FlyFire();
        }
        if (IsCandle)
        {

            if (!Body.activeSelf)
            {

                IsCandle = false;
            }
            Vector3 BodyPos = Body.transform.position;
            BodyPos.y += 0.95f;
            transform.position = BodyPos;
            BigFire();
            MoveFire();


        }
        if (!IsCandle)//炎がロウソクについてないとき
        {

            NoCandle();
        }

        if (IsChangeCandle)
        {
            RecoveryFire();
        }
    }

    void BigFire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 CurrentSize = transform.localScale;
            CurrentSize.y = 2f;
            transform.localScale = CurrentSize;

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Vector3 CurrentSize = transform.localScale;
            CurrentSize.y = 1f;
            transform.localScale = CurrentSize;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            CandleScript.Shorten(BurnSpeed2);
        }
        else
        {
            CandleScript.Shorten(BurnSpeed1);
        }
    }

    void MoveFire()
    {
        if (Input.GetKey(KeyCode.A))
        {

            //    CandleConScript.Move(-MoveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //CandleConScript.Move(MoveSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            FlyFire();
        }
    }

    public void FlyFire()
    {
        IsCandle = false;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position += new Vector3(0, 0.6f, 0);

        SurviveTime = 0;
        if (CandleScript != null)
        {
            CandleScript.Sleep();
        }
    }

    void NoCandle()//ロウソクがなくなったとき
    {
        Firetime += Time.deltaTime;
        transform.localScale = StartScale * (1.0f - Firetime / SurviveTime);
        if (Firetime >= SurviveTime)
        {
            if (CandleScript == null)
            {
                Debug.Log("GAMEOVER");
            }
            if (CandleScript != null)
            {
                // IsCandle = true;
                CandleScript.WakeUp();
                transform.localScale = StartScale;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-Firespeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(Firespeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, Firespeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -Firespeed * Time.deltaTime, 0);
        }
    }
    public void RecoveryFire()//転生
    {
        IsCandle = true;
        CandleScript.WakeUp();
        transform.localScale = StartScale;
        if (TargetObject != null)
        {
            MainCandle.transform.parent.gameObject.SetActive(true);
            MainCandle.transform.position = TargetObject.transform.position;
            MainCandle.transform.localScale = new Vector3(1, 1, 1);
            MainCandle.transform.parent.position = TargetObject.transform.position;
            CandleScript.WakeUp();
            TargetObject.transform.parent.gameObject.SetActive(false);

        }
        IsChangeCandle = false;

    }

    public bool GetIsCandle()
    {
        return IsCandle;
    }

    public bool JudgeCandle
    {
        get { return IsCandle; }
        set { IsCandle = value; }
    }

    public bool JudgeIsChange//転生準備完了したか
    {
        get { return IsChangeCandle; }
        set { IsChangeCandle = value; }
    }

    public GameObject SetObject//転生座標セット
    {

        set { TargetObject = value; }
    }
}
