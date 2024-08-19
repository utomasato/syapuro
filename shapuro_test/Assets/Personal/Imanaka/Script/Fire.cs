using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject PL;
    [SerializeField]
    private float MoveSpeed;//ロウソクについている時の通常移動スピード
    [SerializeField]
    private float DashMoveSpeed;//ロウソクについている時のダッシュ移動スピード
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
    private bool IsNormal = true;

    [Tooltip("現段階では最初に憑依するcandleを参照してください")]
    [SerializeField]
    private Candle CandleScript;



    private Vector3 StartScale;
    [Tooltip("現段階では最初に憑依するcandleを参照してください")]


    /*転生用変数*/
    private bool IsChangeCandle = false;//転生できているか　未完成

    [SerializeField] private GameState gameState;
    [SerializeField] private GaugeController gaugeControllor; // 20240803 宇藤追加
    [SerializeField] private FooterUI footer;


    private AudioSource SE;//ジャンプ用効果音

    private AudioSource SE2;//弱火と強火用効果音


    [SerializeField] private AudioClip JumpSE;//ジャンプの効果音

    [SerializeField] private AudioClip NormalBurnSE;//通常火の効果音

    [SerializeField] private AudioClip StrongBurnSE;//強火の効果音
    [SerializeField] private AudioClip TransferSE;//転生のために火がロウソクを離れた時の音
    [SerializeField] private AudioClip CandleSetSE;//転生完了の音

    // Start is called before the first frame update
    void Start()
    {
        StartScale = transform.localScale;
        Transfer();
        SE = GetComponent<AudioSource>();
        SE.volume = 0.1f;
        GameObject FireSE = GameObject.Find("Fire");
        if (SE2 == null)
        {//Playerの子オブジェクトのFireに弱火と強火のSEを入れる
            SE2 = FireSE.AddComponent<AudioSource>();
        }
        SE2 = FireSE.GetComponent<AudioSource>();
        SE2.volume = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameState.JudgeState("GamePlay"))
            return;

        Vector3 CurrentScale = transform.localScale;
        if (CurrentScale.y <= 0.1f)
        {
            //CandleScript.BurnOut();
        }
        if (CandleScript == null)
        {
            FlyFire();
        }
        if (IsCandle)//炎がロウソクについている時
        {
            transform.position = CandleScript.GetHeadPosition();


            BigFire();
            MoveFire();

            gaugeControllor.UpdateCandleGauge(CandleScript.GetLife()); // 20240803 宇藤追加
            footer.SetJumpTextGrayOut(!CandleScript.GetCanJump());
        }
        if (!IsCandle)//炎がロウソクについてないとき
        {

            NoCandle();
        }

        if (IsChangeCandle)
        {
            //     RecoveryFire();
        }

        if (!IsNormal && SE2.isPlaying)
        {
            if (SE2.time >= 1.7f)
            {
                SE2.time = 1.5f;
            }
        }
        if (IsNormal && SE2.isPlaying)
        {
            if (SE2.time >= 1.7f)
            {
                SE2.time = 1.5f;
            }
        }

    }

    void BigFire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StrongBurnSE_Func();
            IsNormal = false;
            Vector3 CurrentSize = transform.localScale;
            CurrentSize.y *= 2f;
            transform.localScale = CurrentSize;

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SE2.Stop();
            IsNormal = true;
            transform.localScale = StartScale;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            CandleScript.Shorten(Strong_BurnSpeed);
        }
        else
        {
            if (!SE2.isPlaying)
            {
                NormalBurnSE_Func();
            }
            CandleScript.Shorten(Normal_BurnSpeed);
        }
    }

    void MoveFire()//ロウソクに炎がついてる時の移動
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D))// AとDが同時に押されてる時は動かない
            {
                if (Input.GetKey(KeyCode.A))
                {
                    CandleScript.Move(-MoveSpeed);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    CandleScript.Move(MoveSpeed);
                }
                else
                {
                    CandleScript.Move(0);
                }
            }
            else
            {
                CandleScript.Move(0);
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D))// AとDが同時に押されてる時は動かない
            {
                if (Input.GetKey(KeyCode.A))
                {
                    CandleScript.Move(-DashMoveSpeed);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    CandleScript.Move(DashMoveSpeed);
                }
                else
                {
                    CandleScript.Move(0);
                }
            }
            else
            {
                CandleScript.Move(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CandleScript.Jump();
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            CandleScript.StopJump();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //IsCandle = false;
            //CandleScript.Sleep();
            FlyFire();
        }
    }

    public void FlyFire()//蝋燭から離れる
    {
        gameTransferSE();
        IsCandle = false;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position += new Vector3(0, 0, -1);

        Firetime = 0;
        if (CandleScript != null)
        {
            CandleScript.Sleep();
        }
        gaugeControllor.SetCandleGaugeGrayOut(true); // 20240803 宇藤追加
        footer.SwitchInstructionTexts(false);
    }

    void NoCandle()//ロウソクがなくなったとき
    {
        Firetime += Time.deltaTime;
        transform.localScale = StartScale * (1.0f - Firetime / SurviveTime);
        gaugeControllor.UpdateFireGauge(1.0f - Firetime / SurviveTime); // 20240803 宇藤追加
        if (Firetime >= SurviveTime)
        {
            if (CandleScript != null)
            {
                /* 20240803 宇藤コメントアウト
                IsCandle = true;
                CandleScript.WakeUp();
                transform.localScale = StartScale;
                */
                Transfer();
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

    public void Transfer(Candlewick NewCandle = null)//蝋燭に憑依
    {
        if (NewCandle != null)
        {
            if (NewCandle.candle == CandleScript)
            {
                return;
            }
            CandleScript = NewCandle.candle;
        }
        // SE.PlayOneShot(CandleSetSE);
        IsCandle = true;
        CandleScript.WakeUp();
        transform.localScale = StartScale;
        gaugeControllor.SetCandleGaugeGrayOut(false); // 20240803 宇藤追加
        gaugeControllor.FillFireGauge();
        footer.SwitchInstructionTexts(true);
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

    public Candle UseCandle()//どのロウソクを使用しているか
    {


        return CandleScript;

    }
    public void Deletefire()//ゲームオーバーに？？
    {
        //ゲームオーバースクリプトを設定している場合は、BurnOut()のコメントを解除してください

        PL.SetActive(false);
        //CandleScript.BurnOut();
    }
    public bool getIsNormal()
    {
        return IsNormal;
    }
    public bool CanLampOn()
    {
        return IsCandle;
    }

    public void JumpSE_Func()
    {
        SE.volume = 0.1f;
        SE.PlayOneShot(JumpSE);
    }
    public void NormalBurnSE_Func()
    {
        SE2.clip = NormalBurnSE;
        SE2.time = 1.5f;
        SE2.volume = 0.4f;
        SE2.Play();

    }
    public void StrongBurnSE_Func()
    {
        SE2.clip = StrongBurnSE;
        SE2.time = 1.5f;
        SE2.volume = 0.6f;
        SE2.Play();

    }
    void gameTransferSE()
    {
        SE.volume = 0.05f;
        SE.PlayOneShot(TransferSE);
    }
    public Candle GetCandle()
    {

        return CandleScript;
    }
}
