using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    [SerializeField]
    private GameObject PL;
    private float MoveSpeed;//ロウソクについている時の移動スピード
    [SerializeField]
    private float Normal_BurnSpeed;//通常時
    [SerializeField]
    private float Strong_BurnSpeed;//強火時
    [SerializeField]
    private float Firespeed;//火の玉状態のスピード
    [Tooltip("この時間を過ぎたらゲームオーバーになります")]
    [SerializeField]
    private float SurviveTime;//火の玉状態の生存期間

    private float Firetime;//火の玉状態になってからの生存期間

    private bool IsCandle = true;//ロウソクに炎がついているか

    [Tooltip("現段階では最初に憑依するcandleを参照してください")]
    [SerializeField]
    private Candle CandleScript;



    private Vector3 StartScale;
    [Tooltip("現段階では最初に憑依するcandleを参照してください")]
    [SerializeField]
    private GameObject CurrentCandle;


    /*転生用変数*/
    private bool IsChangeCandle = false;//転生できているか　未完成

    private GameObject TargetObject;//使うかわからん
    /*   */
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
        if (IsCandle)//炎がロウソクについている時
        {
            transform.position = CandleScript.GetHedPosition();


            BigFire();
            MoveFire();
            if (!CurrentCandle.activeSelf)
            {
                IsCandle = false;
            }

        }
        if (!IsCandle)//炎がロウソクについてないとき
        {

            NoCandle();
        }

        if (IsChangeCandle)
        {
            //     RecoveryFire();
        }
    }

    void BigFire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 CurrentSize = transform.localScale;
            CurrentSize.y *= 2f;
            transform.localScale = CurrentSize;

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            transform.localScale = StartScale;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            CandleScript.Shorten(Strong_BurnSpeed);
        }
        else
        {
            CandleScript.Shorten(Normal_BurnSpeed);
        }
    }

    void MoveFire()//ロウソクに炎がついてる時の移動
    {
        if (Input.GetKey(KeyCode.A))
        {

            CandleScript.Move(-MoveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            CandleScript.Move(MoveSpeed);
        }
        if (Input.GetKey(KeyCode.W))
        {
            CandleScript.Jump();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            IsCandle = false;
            CandleScript.Sleep();
            // FlyFire();
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

            CandleScript.WakeUp();
            TargetObject.transform.parent.gameObject.SetActive(false);

        }
        IsChangeCandle = false;

    }

    public bool GetIsCandle()
    {
        return IsCandle;
    }


    public bool JudgeCandle //ロウソクについているか
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

    public void DestroyFire()
    {
        PL.SetActive(false);
    }
}
