using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private float interval; // ステージ選択の間隔
    int selectNumber = 0; // 選択されているステージの番号
    bool IsMoving = false; // ステージ選択が移動中かどうか
    float t = 0.0f; // 補間のための時間
    int p0; // 現在の位置
    [SerializeField] Vector3 startPos; // スタート位置
    [SerializeField] private List<string> Stagelist; // ステージのリスト
    [SerializeField] private Fade fade;

    void Start()
    {
        if (SceneSelectionState.selectedIndex == -1)
        {
            p0 = -1; // 初期位置を0に設定
            selectNumber = 0; // 選択番号を設定
            //transform.position = startPos; // スタート位置に設定
        }
        else
        {
            p0 = SceneSelectionState.selectedIndex; // 前回の選択位置を取得

            selectNumber = p0; // 選択番号を設定
        }
        Vector3 pos = startPos;
        pos.x = startPos.x + p0 * interval; // 選択位置に応じてX座標を調整
        transform.position = pos;
        IsMoving = false; // 移動中フラグをリセット
    }

    void Update()
    {
        if (!IsMoving)
        {
            // 右キーが押された場合
            if (Input.GetKeyDown(KeyCode.D) && selectNumber + 1 < Stagelist.Count)
            {
                Move(1);
            }
            // 左キーが押された場合
            if (Input.GetKeyDown(KeyCode.A) && 0 < selectNumber)
            {
                Move(-1);
            }

            // エンターキーが押された場合
            if (Input.GetKeyDown(KeyCode.Return) && 0 <= selectNumber && selectNumber < Stagelist.Count && Stagelist[selectNumber] != null)
            {
                SceneSelectionState.selectedIndex = selectNumber; // 現在の選択番号を保存
                fade.FadeOutStart(Stagelist[selectNumber]);
                //SceneManager.LoadScene(Stagelist[selectNumber]); // 選択されたステージをロード
            }
        }
        else
        {
            t += Time.deltaTime; // 補間の時間を更新
            if (t >= 1.0f || p0 == selectNumber)
            {
                t = 1.0f; // 補間時間の上限を1.0に設定
                IsMoving = false; // 移動中フラグをリセット
                p0 = selectNumber; // 現在の位置を更新
            }
            Vector3 pos = transform.position;
            pos.x = startPos.x + Mathf.Lerp(p0 * interval, selectNumber * interval, t); // 補間を用いてX座標を計算
            transform.position = pos; // 新しい位置を設定
        }
    }

    public void Move(int delta)
    {
        selectNumber += delta; // 選択番号を増やす
        IsMoving = true; // 移動中フラグを設定
        t = 0.0f; // 補間の時間をリセット
    }
}
