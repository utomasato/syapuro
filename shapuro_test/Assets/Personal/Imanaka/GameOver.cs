using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOvercanvas;

    private bool IsGameOver = false;

    [SerializeField]
    private GameObject PL;

    private Vector3 currentScale;

    private bool IsgameStart = false;
    // Start is called before the first frame update
    void Start()
    {
        GameOvercanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("Test", 2.0f);
        if (IsgameStart)
        {
            currentScale = PL.transform.localScale;
            /*  if (currentScale.y < 0)
              {
                  IsGameOver = true;
                  IsgameStart = false;
              }*/
        }
        if (IsGameOver)
        {
            GameOvercanvas.SetActive(true);
        }
    }

    void Test()
    {
        IsGameOver = true;
    }
}

