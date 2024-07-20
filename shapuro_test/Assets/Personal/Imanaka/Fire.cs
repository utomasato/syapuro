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

    [Tooltip("現段階では最初に憑依するcandleを参照してください")]
    [SerializeField]
    private Candle CandleScript;



    private Vector3 StartScale;
    [Tooltip("現段階では最初に憑依するcandleを参照してください")]


    [SerializeField]
    private GameState GameStateScript;
    // Start is called before the first frame update
    void Start()
    {
        StartScale = transform.localScale;
        Transfer(null);

    }

    // Update is called once per frame
    void Update()
    {
        //   if (GameStateScript.GetIsGameStart())
        // {

        Vector3 CurrentScale = transform.localScale;
        if (CurrentScale.y <= 0.1f)
        {
            CandleScript.BurnOut();
        }
        if (CandleScript == null)
        {
            FlyFire();
        }
        if (IsCandle)//炎がロウソクについている時
        {
            transform.position = CandleScript.GetHedPosition();


            BigFire();
            MoveFire();

        }
        if (!IsCandle)//炎がロウソクについてないとき
        {

            NoCandle();
        }

        //   }
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
        if (!Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.A))
            {
                CandleScript.Move(-MoveSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                CandleScript.Move(MoveSpeed);
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.A))
            {
                CandleScript.Move(-DashMoveSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                CandleScript.Move(DashMoveSpeed);
            }
        }
        if (Input.GetKey(KeyCode.W))
        {

            CandleScript.Jump();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //IsCandle = false;
            //CandleScript.Sleep();
            FlyFire();
        }
    }

    public void FlyFire()
    {
        IsCandle = false;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position += new Vector3(0, 0.6f, 0);

        Firetime = 0;
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
            if (CandleScript != null)
            {
                IsCandle = true;
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

    public void Transfer(Candlewick NewCandle)//蝋燭に憑依
    {
        if (NewCandle != null)
        {
            CandleScript = NewCandle.candle;
        }
        IsCandle = true;
        CandleScript.WakeUp();
        transform.localScale = StartScale;
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



    public Candle UseCandle()//どのロウソクを使用しているか
    {

        return CandleScript;

    }
    public void Deletefire()//ゲームオーバーに？？
    {
        //ゲームオーバースクリプトを設定している場合は、コメントを解除してください
        /* if (GameStateScript != null)
                {
                    GameStateScript.JudgeGameOver = true;
                }*/
        PL.SetActive(false);

    }

}
