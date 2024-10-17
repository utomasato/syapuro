using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Candle : MonoBehaviour
{
    [SerializeField] private float startLife; // 初期ライフ
    private float life; // 現在のライフ
    [SerializeField] private float panicLife; // 焦り出すライフ
    [SerializeField] private float startSize; // 初期サイズ
    private float size; // 現在のサイズ 
    [SerializeField] private float minJumpPower, maxJumpPower; // 最小ジャンプ力と最大ジャンプ力
    [SerializeField] private GameObject head; // ロウソクの頭部
    [SerializeField] private GameObject hand; // ロウソクの腕
    [SerializeField] private GameObject body; // ロウソクの胴体
    [SerializeField] private GameObject foot; // ロウソクの足
    [SerializeField] private float handCoefficient; // 腕の位置補正値
    [SerializeField] private float footSize; // 足が生えたときの高さの増加分
    [SerializeField] private float marginSize; // 燃え尽きる時のサイズ
    [SerializeField] private float puttingSize = 0.5f; // 置いているときの追加の長さ

    [Tooltip("HitPointプレファブの中にあるスライドを選択してください")]
    [SerializeField] private Slider CurrentHPbar;//現在のロウソクのHP
    [SerializeField] private GameState gameState;//燃え尽きたらゲームオーバーになる処理

    [SerializeField] private Fire fire;//効果音を入れるため

    private bool IsBurning = false; // ロウソクが燃えているかどうか
    private bool IsGrounded; // 接地しているか
    private bool IsGoingUp; // 上昇中(減速前)か
    private bool IsBurnOut = false; // ロウソクが燃え尽きたかどうか
    private Rigidbody rb; // 物理挙動のためのRigidbodyコンポーネント
    private bool IsRightFacing = true;

    [SerializeField] private List<Animator> animatorList;
    private float speed;
    private float animationSpeed;
    private bool animatiorIsPlaying = true;
    [SerializeField] private Animator effectAnimator;

    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;
    private bool isPaused = false;
    private bool IsPanic = false;

    [SerializeField] float BurningSpeedInTheFlameWall;

    void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
        life = startLife; // 初期ライフを設定
        if (!IsBurning) // 置かれている
        {
            //startSize = transform.lossyScale.y; // 初期サイズを設定
            Vector3 pos = transform.position;
            pos.z = 2;
            transform.position = pos;
            foreach (Animator animator in animatorList)
            {
                animator.SetBool("IsBurning", false);
            }
            size = startSize + puttingSize; // 現在のサイズを初期サイズに設定
            Vector3 ls = transform.localScale;
            ls.y = size;
            transform.localScale = ls;
            body.transform.localScale = ls;
        }
        else // 火がついている
        {
            //startSize = transform.lossyScale.y - footSize - marginSize;
            Shorten(0.0f); // 諸々の大きさと位置を合わすならこれが楽
        }

        foreach (Animator animator in animatorList)
        {
            animator.SetBool("InAir", false);
        }

        rb = GetComponent<Rigidbody>(); // Rigidbodyコンポーネントを取得
        hand.transform.position = new Vector3(0.0f, -100.0f, 0.0f); // 腕を画面外に移動
        foot.transform.position = new Vector3(0.0f, -100.0f, 0.0f); // 足を画面外に移動
    }

    void Update()
    {
        body.transform.position = transform.position; // 胴体の位置をロウソクの位置に合わせる
        if (IsBurning)
        {
            // ロウソクが燃えているときの胴体と足、腕、頭の位置を調整
            body.transform.position += new Vector3(0.0f, footSize / 2.0f, 0.0f);
            foot.transform.position = body.transform.position - new Vector3(0.0f, size / 2.0f, 0.0f);
            hand.transform.position = foot.transform.position + new Vector3(0.0f, size * handCoefficient, 0.0f);
            head.transform.position = body.transform.position + new Vector3(0.0f, size / 2.0f, 0.0f);
            if (speed == 0.0f)
            {
                if (!IsPanic)
                {
                    body.transform.position += new Vector3(0.0f, 0.1f - (1.3f - size) * 0.2f, 0.0f);
                    foot.transform.position += new Vector3(0.0f, 0.1f, 0.0f);
                }
                else
                {
                    body.transform.position += new Vector3(0.0f, -0.05f + (1.3f - size) * 0.07f, 0.0f);
                    head.transform.position += new Vector3(0.0f, -0.1f, 0.0f);
                }

            }

            if (IsGrounded && Mathf.Abs(rb.velocity.y) < 0.5f) // 接地している時
            {
                foreach (Animator animator in animatorList)
                {
                    animator.SetBool("InAir", false);
                }
            }
            else // 接地していない時
            {
                foreach (Animator animator in animatorList)
                {
                    animator.SetBool("InAir", true);
                }
                if (IsGoingUp && rb.velocity.y < -0.1f) // 下降に転じた時
                {
                    IsGoingUp = false;
                }
                if (Mathf.Abs(rb.velocity.y) < 0.1f) // 上下動が小さい時
                {
                    if (Physics.Raycast(transform.position, Vector3.down, transform.localScale.y / 2f))
                    {
                        IsGrounded = true;
                        foreach (Animator animator in animatorList)
                        {
                            animator.SetBool("InAir", false);
                        }
                    }
                }
            }
        }
        else
        {
            head.transform.position = body.transform.position + new Vector3(0.0f, size / 2.0f, 0.0f); // 頭部の位置を正しく設定
        }
    }

    public void Shorten(float burningSpeed) // ロウソクを短くする（燃焼速度に応じて）
    {
        life -= burningSpeed * Time.deltaTime; // ライフを減らす
        size = startSize * (life / startLife) + marginSize; // ライフに応じてサイズを更新
        if (life <= 0.0f)
        {
            if (!IsBurnOut) BurnOut(); // ライフが0になったら燃え尽き処理を実行
            return;
        }
        if (!IsPanic && life <= panicLife)
        {
            effectAnimator.Play("PanicEffect");
            IsPanic = true;
            foreach (Animator animator in animatorList)
            {
                animator.Play("Stop");
                animator.Play("PanicStop");
            }

        }

        Vector3 ls = transform.localScale; // ローカルスケールを更新
        ls.y = size;
        /* if (ls.y <= 1.0)
         {
             fire.gameWarningSE();
         }*/
        if (IsRightFacing)
            ls.x = 1.0f;
        else
            ls.x = -1.0f;
        body.transform.localScale = ls;

        if (IsBurning)
            ls.y += footSize; // 燃えている場合はサイズを調整
        transform.localScale = ls;
    }

    public void Move(float x) // ロウソクを水平に移動
    {
        //transform.position += new Vector3(x * Time.deltaTime, 0, 0);
        Vector3 v = rb.velocity;
        v.x = x;
        rb.velocity = v;
        if (0 < x)
            IsRightFacing = true;
        else if (x < 0)
            IsRightFacing = false;
        foreach (GameObject obj in new List<GameObject>() { head, body, hand, foot })
        {
            Vector3 ls = obj.transform.localScale;
            if (0 < x)
                ls.x = 1.0f;
            else if (x < 0)
                ls.x = -1.0f;
            obj.transform.localScale = ls;
        }
        speed = x;
        foreach (Animator animator in animatorList)
        {
            animator.SetFloat("speed", Mathf.Abs(speed));
        }
    }

    public void Jump() // ロウソクをジャンプさせる
    {
        float jumpPower = Mathf.Lerp(minJumpPower, maxJumpPower, 1.0f - life / startLife); // ライフに応じてジャンプ力を計算
        if (IsGrounded && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            fire.JumpSE_Func();
            rb.velocity = Vector3.up * jumpPower; // ジャンプ力を適用
            IsGrounded = false; // 衝突が再び検出されるまでジャンプ不可にする
            IsGoingUp = true;
            foreach (Animator animator in animatorList)
            {
                animator.SetBool("InAir", true);
            }
        }
    }

    public void StopJump()
    {
        if (IsGoingUp && rb.velocity.y > 0.1f)
        {
            rb.velocity *= 0.3f;
            IsGoingUp = false;
        }
    }

    public void WakeUp() // 足を生やして位置を調整
    {
        if (IsBurning) return; // すでに憑依状態だったら何もしない
        IsBurning = true;
        Vector3 pos = transform.position;
        pos.z = 0;
        pos.y += (footSize - puttingSize) / 2; // 足の大きさに応じて位置を調整
        transform.position = pos;

        size = startSize * (life / startLife) + marginSize;

        Vector3 ls = transform.localScale;
        ls.y = size + footSize; // 足の大きさに応じてローカルスケールを調整
        transform.localScale = ls;

        foreach (Animator animator in animatorList) // 蝋燭の見た目を憑依状態にする
        {
            animator.SetBool("IsBurning", true);
            animator.SetBool("InAir", false);
            animator.SetFloat("speed", 0f);

            if (IsPanic)
                animator.Play("PanicStop");
            else
                animator.Play("Stop");
        }

        if (IsPanic)
        {
            effectAnimator.Play("PanicEffect");
        }
    }

    public void Sleep() // 足を消して位置を調整
    {
        if (!IsBurning) return;
        IsBurning = false;

        Vector3 v = rb.velocity;
        v.x = 0f;
        rb.velocity = v;

        Vector3 pos = transform.position;
        pos.z = 2;
        pos.y += footSize / 2; // 足の消失に応じて位置を調整
        transform.position = pos;

        size = startSize * (life / startLife) + puttingSize;

        Vector3 ls = transform.localScale;
        ls.y = size; // ローカルスケールをリセット
        transform.localScale = ls;
        body.transform.localScale = ls;
        hand.transform.position = new Vector3(0.0f, -100.0f, 0.0f); // 腕を画面外に移動
        foot.transform.position = new Vector3(0.0f, -100.0f, 0.0f); // 足を画面外に移動
        if (CurrentHPbar != null)
        {
            CurrentHPbar.value = 1;//HPバーリセット
        }

        foreach (Animator animator in animatorList) // 蝋燭の見た目を抜け殻状態にする
        {
            animator.SetBool("IsBurning", false);
            animator.SetBool("InAir", false);
        }
        effectAnimator.Play("Stop");
    }

    private void BurnOut() // ロウソクの燃え尽き処理
    {
        Debug.Log(this.name + " : BurnOut"); // 燃え尽きたことをログに出力
        IsBurnOut = true;
        if (CurrentHPbar != null)
        {
            CurrentHPbar.gameObject.SetActive(false);
        }
        if (gameState != null && IsBurning)
        {
            gameState.GameOver();
        }
        transform.parent.gameObject.SetActive(false); // 親オブジェクトを非アクティブ化

    }

    public void Pause()
    {
        if (!isPaused)
        {
            StopAnimation();
            PauseRigidbody();
            isPaused = true;
            IsGoingUp = false;
        }
    }

    public void Resume()
    {
        if (isPaused)
        {
            PlayAnimation();
            ResumeRigidbody();
            isPaused = false;
        }
    }

    private void StopAnimation() // アニメーションを停止させる
    {
        if (!animatiorIsPlaying) // すでに停止していたら何もしない
            return;
        animationSpeed = animatorList[0].speed; // 停止する前の再生速度を保存する
        foreach (Animator animator in animatorList) // アニメーションの速度を0にする
        {
            animator.speed = 0.0f;
        }
        animatiorIsPlaying = false;
        effectAnimator.speed = 0.0f;
    }

    private void PlayAnimation() // アニメーションを再生する
    {
        foreach (Animator animator in animatorList) // アニメーションの速度を元に戻す
        {
            animator.speed = animationSpeed;
        }
        animatiorIsPlaying = true;
        effectAnimator.speed = 1.0f;
    }

    private void PauseRigidbody()
    {
        // 現在の速度と回転速度を保存
        savedVelocity = rb.velocity;
        savedAngularVelocity = rb.angularVelocity;
        // Rigidbodyをキネマティックにして一時停止
        rb.isKinematic = true;
    }

    public void ResumeRigidbody()
    {
        // Rigidbodyを非キネマティックに戻し、保存した速度と回転速度を復元
        rb.isKinematic = false;
        rb.velocity = savedVelocity;
        rb.angularVelocity = savedAngularVelocity;
    }

    public float GetSize() // 現在のサイズを返す
    {
        return size;
    }

    public float GetLife() // 残りのライフを返す
    {
        return life;
    }

    public Vector3 GetHeadPosition() // 頭の位置を返す
    {
        return head.transform.position;
    }

    public bool GetBurnOut() // 蝋燭が燃え尽きているかを返す
    {
        return IsBurnOut;
    }

    public bool GetCanJump() // ジャンプ可能か
    {
        return IsGrounded && Mathf.Abs(rb.velocity.y) < 0.5f;
    }

    void OnCollisionEnter(Collision other)
    {
        // 接触点の法線ベクトルが上向きであるかどうかをチェック
        foreach (ContactPoint contact in other.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                IsGrounded = true;
                foreach (Animator animator in animatorList)
                {
                    animator.SetBool("InAir", false);
                }
                break; // 上向きの法線ベクトルが見つかったらループを抜ける
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        foreach (Animator animator in animatorList)
        {
            animator.SetBool("InAir", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("fire") && gameState.JudgeState("GamePlay"))
        {
            fire.FireWallSE();

        }
        // 火がついているときにゴールに触れたらクリア
        if (other.gameObject.CompareTag("Goal") && IsBurning)
        {
            gameState.GameClear();
            /*foreach (Animator animator in animatorList)
            {
                animator.SetFloat("speed", 0.0f);
            }*/
            Move(0.0f);
        }
    }
    private void OnTriggerStay(Collider other)
    { // 炎の壁に当たると短くなる
        if (other.gameObject.CompareTag("fire") && gameState.JudgeState("GamePlay"))
        {
            Shorten(BurningSpeedInTheFlameWall * fire.GetBurnRate);
        }
    }

    private void OnTriggerExit(Collider other)
    { // 炎の壁に当たると短くなる
        if (other.gameObject.CompareTag("fire") && gameState.JudgeState("GamePlay"))
        {
            fire.SEstop();
        }
    }

    public float HPbar()
    {
        return size / startSize;
    }
}
