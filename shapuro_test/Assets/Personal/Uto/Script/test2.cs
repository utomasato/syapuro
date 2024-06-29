// StageSelect.cs,SceneSelectionState.csの動作確認
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test2 : MonoBehaviour
{
    [SerializeField] private string SelectScene;
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SelectScene);
        }
    }
}
