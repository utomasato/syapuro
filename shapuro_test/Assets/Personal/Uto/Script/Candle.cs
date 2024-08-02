using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Candle : MonoBehaviour
{
    [SerializeField] private float startLife; // 初期ライフ
    private float life; // 現在のライフ
    private float startSize; // 初期サイズ
    [SerializeField] private float size; // 現在のサイズ Start()でWakeUp()が呼び出されたときのために入力が必要
    [SerializeField] private float minJumpPower, maxJumpPower; // 最小ジャンプ力と最大ジャンプ力
    [SerializeField] private GameObject head; // ロウソクの頭部
    [SerializeField] private GameObject hand; // ロウソクの腕
    [SerializeField] private GameObject body; // ロウソクの胴体
    [SerializeField] private GameObject foot; // ロウソクの足
    [SerializeField] private float handCoefficient; // 腕の位置補正値
    [SerializeField] private float footSize; // 足が生えたときの高さの増加分
    [SerializeField] private float marginSize; // 燃え尽きる時のサイズ

    [Tooltip("HitPointプレファブの中にあるスライドを選択してください")]
    [SerializeField] private Slider CurrentHPbar;//現在のロウソクのHP
    [SerializeField] private GameState GameStateScript;//燃え尽きたらゲームオーバーになる処理

    private bool IsBurning = false; // ロウソクが燃えているかどうか
    private bool CanJump; // ジャンプ可能かどうか
    private bool IsBurnOut = false; // ロウソクが燃え尽きたかどうか
    private Rigidbody rb; // 物理挙動のためのRigidbodyコンポーネント
    private bool IsRightFacing = true;

    [SerializeField] private List<Animator> animatorList;
    private float speed;


    void Start()
    {
        life = startLife; // 初期ライフを設定
        if (!IsBurning)
        {
            startSize = transform.lossyScale.y; // 初期サイズを設定
            Vector3 pos = transform.position;
            pos.z = 2;
            transform.position = pos;
            foreach (Animator animator in animatorList)
            {
                animator.SetBool("IsBurning", false);
            }
        }
        else
            startSize = transform.lossyScale.y - footSize - marginSize; ;
        size = startSize; // 現在のサイズを初期サイズに設定

        rb = GetComponent<Rigidbody>(); // Rigidbodyコンポーネントを取得
        hand.transform.position = new Vector3(0.0f, -100.0f, 0.0f); // 腕を画面外に移動
        foot.transform.position = new Vector3(0.0f, -100.0f, 0.0f); // 足を画面外に移動
    }

    void Update()
    {
        body.transform.position = transform.position; // 胴体の位置をロウソクの位置に合わせる
        if (IsBurning)
        {
            // ロウソクが燃えているときの胴体と足、腕の位置を調整
            body.transform.position += new Vector3(0.0f, footSize / 2.0f, 0.0f);
            foot.transform.position = body.transform.position - new Vector3(0.0f, size / 2.0f, 0.0f);
            hand.transform.position = foot.transform.position + new Vector3(0.0f, size * handCoefficient, 0.0f);
            head.transform.position = body.transform.position + new Vector3(0.0f, size / 2.0f, 0.0f);
            if (speed == 0.0f)
            {
                body.transform.position += new Vector3(0.0f, 0.1f - (startSize - size) * 0.2f, 0.0f);
                foot.transform.position += new Vector3(0.0f, 0.1f, 0.0f);
            }
            foreach (Animator animator in animatorList)
            {
                animator.SetFloat("speed", Mathf.Abs(speed));
            }
            speed = 0.0f;
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
        Vector3 ls = transform.localScale; // ローカルスケールを更新
        ls.y = size;
        if (IsRightFacing)
            ls.x = 1.0f;
        else
            ls.x = -1.0f;
        body.transform.localScale = ls;

        if (IsBurning)
            ls.y += footSize; // 燃えている場合はサイズを調整
        transform.localScale = ls;
        if (CurrentHPbar != null)
        {
            CurrentHPbar.value = HPbar();
        }

    }

    public void Move(float x) // ロウソクを水平に移動
    {
        transform.position += new Vector3(x * Time.deltaTime, 0, 0);
        if (0 < x)
            IsRightFacing = true;
        else
            IsRightFacing = false;
        foreach (GameObject obj in new List<GameObject>() { head, body, hand, foot })
        {
            Vector3 ls = obj.transform.localScale;
            if (0 < x)
                ls.x = 1.0f;
            else
                ls.x = -1.0f;
            obj.transform.localScale = ls;
        }
        speed = x;
    }

    public void Jump() // ロウソクをジャンプさせる
    {
        float jumpPower = Mathf.Lerp(minJumpPower, maxJumpPower, 1.0f - life / startLife); // ライフに応じてジャンプ力を計算
        if (CanJump)
        {
            rb.velocity = Vector3.up * jumpPower; // ジャンプ力を適用
            CanJump = false; // 衝突が再び検出されるまでジャンプ不可にする
        }
    }

    public void WakeUp() // 足を生やして位置を調整
    {
        if (IsBurning) return;
        IsBurning = true;
        Vector3 pos = transform.position;
        pos.z = 0;
        pos.y += footSize / 2; // 足の大きさに応じて位置を調整
        transform.position = pos;

        size += marginSize;

        Vector3 ls = transform.localScale;
        ls.y = size + footSize; // 足の大きさに応じてローカルスケールを調整
        transform.localScale = ls;

        foreach (Animator animator in animatorList)
        {
            animator.SetBool("IsBurning", true);
        }
    }

    public void Sleep() // 足を消して位置を調整
    {
        if (!IsBurning) return;
        IsBurning = false;
        Vector3 pos = transform.position;
        pos.z = 2;
        pos.y += footSize / 2; // 足の消失に応じて位置を調整
        transform.position = pos;

        size = startSize * (life / startLife);

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

        foreach (Animator animator in animatorList)
        {
            animator.SetBool("IsBurning", false);
        }
    }

    private void BurnOut() // ロウソクの燃え尽き処理
    {
        Debug.Log(this.name + " : BurnOut"); // 燃え尽きたことをログに出力
        IsBurnOut = true;
        if (CurrentHPbar != null)
        {
            CurrentHPbar.gameObject.SetActive(false);
        }
        if (GameStateScript != null)
        {
            GameStateScript.JudgeGameOver = true;
        }
        transform.parent.gameObject.SetActive(false); // 親オブジェクトを非アクティブ化

    }

    public float GetSize() // 現在のサイズを返す
    {
        return size;
    }

    public Vector3 GetHeadPosition()
    {
        return head.transform.position;
    }

    public bool GetBurnOut()
    {
        return IsBurnOut;
    }

    void OnCollisionEnter(Collision other)
    {
        // 接触点の法線ベクトルが上向きであるかどうかをチェック
        foreach (ContactPoint contact in other.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                CanJump = true;
                break; // 上向きの法線ベクトルが見つかったらループを抜ける
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        foreach (ContactPoint contact in other.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                CanJump = false;
                break; // 上向きの法線ベクトルが見つかったらループを抜ける
            }
        }
    }

    public float HPbar()
    {
        return size / startSize;
    }
}
