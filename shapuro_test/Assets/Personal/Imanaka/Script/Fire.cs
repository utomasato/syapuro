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
    [SerializeField] private float Idle_BurnSpeed = 0.02f; //プレイヤー停止時
    [SerializeField]
    private float Normal_BurnSpeed;//通常時
    [SerializeField]
    private float Strong_BurnSpeed;//強火時
    [SerializeField] private List<float> BurnRates = new List<float>() { 0.5f, 1f };
    private float BurnRate;
    [SerializeField]
    private float Firespeed;//火の玉状態のスピード
    [Tooltip("この時間を過ぎたらゲームオーバーになります")]
    [SerializeField]
    private float SurviveTime;//火の玉状態の生存期間

    private float Firetime;//火の玉状態になってからの生存期間

    private bool IsOnCandle = true;//ロウソクに炎がついているか
    private bool IsNormal = true;//火の大きさが普通かどうか

    private bool DidWarning = false;//警告音を出したか
    [Tooltip("現段階では最初に憑依するcandleを参照してください")]
    [SerializeField]
    private Candle CandleScript;
    [SerializeField] private SpriteRenderer fireSpriteRenderer;
    [SerializeField] private float fireAlpha = 128;


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

    [SerializeField] private AudioClip WarningSE;//ロウソクが燃え尽きそうな時になる警告音（必要かわからないが念の為）
    [SerializeField] private AudioClip CandleSetSE;//転生完了の音

    [SerializeField] private AudioClip BurnInTheFireWallSE;//FireWAllに当たった時の音


    private List<string> list = new List<string>() { "shift", "ctrl", "alt", "cmd" };

    private Vector3 PreviousPos;//現在の位置を保存
    [SerializeField] private Animator fireAnimator;

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

        BurnRate = BurnRates[SceneSelectionState.mode];
        Debug.Log(BurnRate);
        KeyBindings.LoadConfig();
        PreviousPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameState.JudgeState("GamePlay"))
        {
            if (gameState.JudgeState("GameClear"))
                transform.position = CandleScript.GetHeadPosition();
            return;
        }

        Vector3 CurrentScale = transform.localScale;
        if (CandleScript == null)
        {
            FlyFire();
        }
        if (IsOnCandle)//炎がロウソクについている時
        {
            transform.position = CandleScript.GetHeadPosition();

            BigFire();
            MoveFire();

            gaugeControllor.UpdateCandleGauge(CandleScript.GetLife()); // 20240803 宇藤追加
            footer.SetJumpTextGrayOut(!CandleScript.GetCanJump());
        }
        if (!IsOnCandle)//炎がロウソクについてないとき
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



        if (GetStay(KeyBindings.DashKay) || GetStay(KeyBindings.DashButton))
        {
            if (IsNormal)
            {
                StrongBurnSE_Func();
                IsNormal = false;
                Vector3 CurrentSize = transform.localScale;
                CurrentSize.y *= 2f;
                transform.localScale = CurrentSize;
            }
            CandleScript.Shorten(Strong_BurnSpeed * BurnRate);
        }
        else
        {
            bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("JoyHorizontal") < -0.1f;
            bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("JoyHorizontal") > 0.1f;


            if ((!moveLeft && !moveRight && CandleScript.GetCanJump()) || moveLeft && moveRight && CandleScript.GetCanJump())
            {
                CandleScript.Shorten(Idle_BurnSpeed * BurnRate);
            }

            else
            {
                CandleScript.Shorten(Normal_BurnSpeed * BurnRate);
            }

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


        }




    }

    void MoveFire()//ロウソクに炎がついてる時の移動
    {
        float speed = 0f;
        if (GetStay(KeyBindings.DashKay) || GetStay(KeyBindings.DashButton))
        {
            speed = DashMoveSpeed;
        }
        else
        {
            speed = MoveSpeed;
        }
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) == (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        && Mathf.Abs(Input.GetAxis("JoyHorizontal")) < 0.1f) // 両方押されているまたは押されていない時
        {
            CandleScript.Move(0f);
        }
        else // 片方のみ押されている時
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("JoyHorizontal") < -0.1f)
            {
                CandleScript.Move(-speed);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("JoyHorizontal") > 0.1f)
            {
                CandleScript.Move(speed);
            }
        }

        if (GetDown(KeyBindings.JumpKay) || GetDown(KeyBindings.JumpButton))
        {
            CandleScript.Jump();
        }
        else if (GetUp(KeyBindings.JumpKay) || GetUp(KeyBindings.JumpButton))
        {
            CandleScript.StopJump();
        }
        if (GetDown(KeyBindings.TransferKay) || GetDown(KeyBindings.TransferButton))
        {
            FlyFire();
        }
    }

    public void FlyFire()//蝋燭から離れる
    {
        gameTransferSE();
        IsOnCandle = false;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position += new Vector3(0, 0, -1);

        Firetime = 0;
        if (CandleScript != null)
        {
            CandleScript.Sleep();
        }
        IsNormal = true;
        gaugeControllor.SetCandleGaugeGrayOut(true); // 20240803 宇藤追加
        footer.SwitchInstructionTexts(false);
        fireSpriteRenderer.color = new Color(255f, 255f, 255f, fireAlpha / 256f);
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
                transform.position = CandleScript.GetHeadPosition();
                Transfer();
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("JoyHorizontal") < -0.5f)
        {
            transform.position += new Vector3(-Firespeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("JoyHorizontal") > 0.5f)
        {
            transform.position += new Vector3(Firespeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("JoyVertical") > 0.5f)
        {
            transform.position += new Vector3(0, Firespeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("JoyVertical") < -0.5f)
        {
            transform.position += new Vector3(0, -Firespeed * Time.deltaTime, 0);
        }

        if ((GetDown(KeyBindings.TransferKay) || GetDown(KeyBindings.TransferButton)) && Firetime > 0.5f) // 転生キャンセル
        {
            transform.position = CandleScript.GetHeadPosition();
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
        //   DidWarning = false;
        IsOnCandle = true;
        CandleScript.WakeUp();
        transform.localScale = StartScale;
        gaugeControllor.SetCandleGaugeGrayOut(false); // 20240803 宇藤追加
        gaugeControllor.FillFireGauge();
        footer.SwitchInstructionTexts(true);
        fireSpriteRenderer.color = new Color(255f, 255f, 255f, 1f);
    }

    public void Transfer(Candle NewCandle)//蝋燭に憑依
    {
        if (NewCandle != null)
        {
            if (NewCandle == CandleScript)
            {
                return;
            }
            gameGetCandleSE();
            CandleScript = NewCandle;
        }
        if (CandleScript.GetBurnOut()) // 憑依した蝋燭が既に燃え尽きていたら
        {
            gameState.GameOver();
        }
        //   DidWarning = false;
        IsOnCandle = true;
        CandleScript.WakeUp();
        transform.localScale = StartScale;
        gaugeControllor.SetCandleGaugeGrayOut(false); // 20240803 宇藤追加
        gaugeControllor.FillFireGauge();
        footer.SwitchInstructionTexts(true);
        fireSpriteRenderer.color = new Color(255f, 255f, 255f, 1f);
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

    public bool GetIsOnCandle()
    {
        return IsOnCandle;
    }

    public bool JudgeIsChange//転生準備完了したか
    {
        get { return IsChangeCandle; }
        set { IsChangeCandle = value; }
    }

    public Candle GetCandle()
    {
        return CandleScript;
    }

    public void Deletefire()//ゲームオーバーに？？
    {
        //PL.SetActive(false);
        FlyFire();
        fireAnimator.Play("death");
        gameState.GameOver();
    }
    public void SetDeathCause(string Cause)
    {
        gameState.SetDeathCause(Cause);
    }
    public bool getIsNormal()
    {
        return IsNormal;
    }

    public float GetBurnRate => BurnRate;



    /*********************以下効果音**************/
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

    public void gameWarningSE()
    {
        if (!DidWarning)
        {
            SE.volume = 0.4f;
            SE.PlayOneShot(WarningSE);
        }
        DidWarning = true;
    }
    public void FireWallSE()
    {
        SE.volume = 0.7f;
        SE.PlayOneShot(BurnInTheFireWallSE);
    }
    public void SEstop()
    {
        SE.Stop();
    }


}
