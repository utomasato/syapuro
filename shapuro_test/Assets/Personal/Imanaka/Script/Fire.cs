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
    private bool IsNormal = true;//火の大きさが普通かどうか

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

    private List<string> list = new List<string>() { "shift", "ctrl", "alt", "cmd" };

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

        KeyBindings.LoadConfig();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameState.JudgeState("GamePlay"))
            return;

        Vector3 CurrentScale = transform.localScale;
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
            gaugeControllor.UpdateCandleGauge(CandleScript.GetLife()); // 20240803 宇藤追加
            gaugeControllor.UpdateFireGauge(1.0f - Firetime / SurviveTime);
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
        bool dashkey;
        if (list.IndexOf(KeyBindings.DashKay) >= 0)
            dashkey = Input.GetKey("left " + KeyBindings.DashKay) || Input.GetKey("right " + KeyBindings.DashKay);
        else
            dashkey = Input.GetKey(KeyBindings.DashKay);
        if (dashkey)
        {
            if (IsNormal)
            {
                StrongBurnSE_Func();
                IsNormal = false;
                Vector3 CurrentSize = transform.localScale;
                CurrentSize.y *= 2f;
                transform.localScale = CurrentSize;
            }
            CandleScript.Shorten(Strong_BurnSpeed);
        }
        if (!dashkey)
        {
            if (!IsNormal)
            {
                SE2.Stop();
                IsNormal = true;
                transform.localScale = StartScale;
            }

            if (!SE2.isPlaying)
            {
                NormalBurnSE_Func();
            }
            CandleScript.Shorten(Normal_BurnSpeed);
        }
    }

    void MoveFire()//ロウソクに炎がついてる時の移動
    {
        float speed = 0f;
        bool dashkey = GetStay(KeyBindings.DashKay);
        /*
        if (list.IndexOf(KeyBindings.DashKay) >= 0)
            dashkey = Input.GetKey("left " + KeyBindings.DashKay) || Input.GetKey("right " + KeyBindings.DashKay);
        else
            dashkey = Input.GetKey(KeyBindings.DashKay);
        */
        if (dashkey)
        {
            speed = DashMoveSpeed;
        }
        else
        {
            speed = MoveSpeed;
        }
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) == (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))) // 両方押されているまたは押されていない時
        {
            CandleScript.Move(0f);
        }
        else // 片方のみ押されている時
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                CandleScript.Move(-speed);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                CandleScript.Move(speed);
            }
        }

        if (GetDown(KeyBindings.JumpKay))
        {
            CandleScript.Jump();
        }
        else if (GetUp(KeyBindings.JumpKay))
        {
            CandleScript.StopJump();
        }
        if (GetDown(KeyBindings.TransferKay))
        {
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

    void NoCandle()//ロウソクがない時の移動
    {
        Firetime += Time.deltaTime;
        transform.localScale = StartScale * (1.0f - Firetime / SurviveTime);

        if (Firetime >= SurviveTime)
        {
            if (CandleScript != null)
            {
                gameGetCandleSE();
                Transfer();
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-Firespeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(Firespeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, Firespeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, -Firespeed * Time.deltaTime, 0);
        }

        if (GetDown(KeyBindings.TransferKay) && Firetime > 0.1f) // 転生キャンセル
        {
            Transfer();
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
            gameGetCandleSE();
            CandleScript = NewCandle.candle;
        }
        if (CandleScript.GetBurnOut()) // 憑依した蝋燭が既に燃え尽きていたら
        {
            gameState.GameOver();
        }

        IsCandle = true;
        CandleScript.WakeUp();
        transform.localScale = StartScale;
        gaugeControllor.SetCandleGaugeGrayOut(false); // 20240803 宇藤追加
        gaugeControllor.FillFireGauge();
        footer.SwitchInstructionTexts(true);
    }

    private bool GetDown(string key)
    {
        if (list.IndexOf(key) >= 0)
            return Input.GetKeyDown("left " + key) || Input.GetKeyDown("right " + key);
        else
            return Input.GetKeyDown(key);
    }
    private bool GetUp(string key)
    {
        if (list.IndexOf(key) >= 0)
            return Input.GetKeyUp("left " + key) || Input.GetKeyUp("right " + key);
        else
            return Input.GetKeyUp(key);
    }
    private bool GetStay(string key)
    {
        if (list.IndexOf(key) >= 0)
            return Input.GetKey("left " + key) || Input.GetKey("right " + key);
        else
            return Input.GetKey(key);
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
        PL.SetActive(false);
        gameState.GameOver();
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
    void gameGetCandleSE()
    {
        SE.Stop();
        SE.volume = 0.2f;
        SE.PlayOneShot(CandleSetSE);
    }
    public Candle GetCandle()
    {

        return CandleScript;
    }
}
