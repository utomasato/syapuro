using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    [SerializeField] private float startLife; // 初期ライフ
    private float life; // 現在のライフ
    private float startSize; // 初期サイズ
    [SerializeField] private float size; // 現在のサイズ Start()でWakeUp()が呼び出されたときのために入力が必要
    [SerializeField] private float minJumpPower, maxJumpPower; // 最小ジャンプ力と最大ジャンプ力
    [SerializeField] private GameObject hed; // ロウソクの頭部
    [SerializeField] private GameObject body; // ロウソクの胴体
    [SerializeField] private GameObject foot; // ロウソクの足
    [SerializeField] private float footSize; // 足が生えたときの高さの増加分

    private bool IsBurning = false; // ロウソクが燃えているかどうか
    private bool CanJump; // ジャンプ可能かどうか
    private bool IsBurnOut = false; // ロウソクが燃え尽きたかどうか
    private Rigidbody rb; // 物理挙動のためのRigidbodyコンポーネント

    void Start()
    {
        life = startLife; // 初期ライフを設定
        if (!IsBurning)
            startSize = transform.lossyScale.y; // 初期サイズを設定
        else
            startSize = transform.lossyScale.y - footSize;
        size = startSize; // 現在のサイズを初期サイズに設定

        rb = GetComponent<Rigidbody>(); // Rigidbodyコンポーネントを取得
        foot.transform.position = new Vector3(0.0f, -100.0f, 0.0f); // 足を画面外に移動
    }

    void Update()
    {
        body.transform.position = transform.position; // 胴体の位置をロウソクの位置に合わせる
        if (IsBurning)
        {
            // ロウソクが燃えているときの胴体と足の位置を調整
            body.transform.position += new Vector3(0.0f, footSize / 2.0f, 0.0f);
            foot.transform.position = body.transform.position - new Vector3(0.0f, size / 2.0f, 0.0f);
        }
        hed.transform.position = body.transform.position + new Vector3(0.0f, size / 2.0f, 0.0f); // 頭部の位置を正しく設定
    }

    public void Shorten(float burningSpeed) // ロウソクを短くする（燃焼速度に応じて）
    {
        life -= burningSpeed * Time.deltaTime; // ライフを減らす
        size = startSize * (life / startLife); // ライフに応じてサイズを更新
        if (life <= 0.0f)
        {
            if (!IsBurnOut) BurnOut(); // ライフが0になったら燃え尽き処理を実行
            return;
        }
        Vector3 ls = transform.localScale; // ローカルスケールを更新
        ls.y = size;
        body.transform.localScale = ls;
        if (IsBurning) ls.y += footSize; // 燃えている場合はサイズを調整
        transform.localScale = ls;
    }

    public void Move(float x) // ロウソクを水平に移動
    {
        transform.position += new Vector3(x * Time.deltaTime, 0, 0);
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
        IsBurning = true;
        Vector3 pos = transform.position;
        pos.y += footSize / 2; // 足の大きさに応じて位置を調整
        transform.position = pos;

        Vector3 ls = transform.localScale;
        ls.y = size + footSize; // 足の大きさに応じてローカルスケールを調整
        transform.localScale = ls;
    }

    public void Sleep() // 足を消して位置を調整
    {
        IsBurning = false;
        Vector3 pos = transform.position;
        pos.y += footSize / 2; // 足の消失に応じて位置を調整
        transform.position = pos;

        Vector3 ls = transform.localScale;
        ls.y = size; // ローカルスケールをリセット
        transform.localScale = ls;
        foot.transform.position = new Vector3(0.0f, -100.0f, 0.0f); // 足を画面外に移動
    }

    public void BurnOut() // ロウソクの燃え尽き処理
    {
        Debug.Log(this.name + " : BurnOut"); // 燃え尽きたことをログに出力
        IsBurnOut = true;
        transform.parent.gameObject.SetActive(false); // 親オブジェクトを非アクティブ化
    }

    public float GetSize() // 現在のサイズを返す
    {
        return size;
    }

    public Vector3 GetHedPosition()
    {
        return hed.transform.position;
    }

    void OnCollisionEnter(Collision other)
    {
        CanJump = true; // 衝突を検出したときジャンプ可能にする
    }

    void OnCollisionExit(Collision other)
    {
        CanJump = false; // 衝突が終了したときジャンプ不可にする
    }
}
