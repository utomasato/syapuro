// StageSelect.cs,SceneSelectionState.csの動作確認
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
    bool a = true;
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SelectScene);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (b)
                state.Pause();
            else
                state.Resume();
            b = !b;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (a)
                candle.StopAnimation();
            else
                candle.PlayAnimation();
            a = !a;
        }
    }
}
