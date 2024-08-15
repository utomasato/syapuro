using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private UnityEngine.UI.Button button;

    public void PauseSystem()
    {
        gameState.SetButton(button);
        pauseCanvas.SetActive(true);
    }

    public void ResumeSystem()
    {
        pauseCanvas.SetActive(false);
    }
}
