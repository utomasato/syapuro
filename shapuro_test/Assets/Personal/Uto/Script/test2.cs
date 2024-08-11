// 動作確認用
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test2 : MonoBehaviour
{
    [SerializeField] private string SelectScene;
    [SerializeField] private GameState state;
    [SerializeField] private Candle candle;
    bool b = true;

    void Update()
    {
        if (Input.GetKey(KeyCode.R)) // セレクト画面に戻る
        {
            SceneManager.LoadScene(SelectScene);
        }
        if (Input.GetKeyDown(KeyCode.P)) // ポーズ画面にする
        {
            if (b)
                state.Pause();
            else
                state.Resume();
            b = !b;
        }
    }
}
