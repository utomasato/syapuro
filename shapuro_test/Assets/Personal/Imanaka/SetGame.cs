using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager
{
    private bool IsGameOver = false;

    public bool GetisGameOver()
    {
        return IsGameOver;
    }
    public void SetisGameOver(bool isGameOver)
    {
        IsGameOver = isGameOver;
    }
}
public class SetGame : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
