using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedFlameWall : MonoBehaviour
{
    [SerializeField] private float interval = 10f;
    private float elapsedTime;
    [SerializeField] private float initialTime;
    private Vector3 size;
    private GameState gameState;

    void Start()
    {
        elapsedTime = 0f;
        size = new Vector3(1f, 1f, 1f);
        gameState = GameObject.Find("GameState").GetComponent<GameState>();

        size.y = Mathf.PingPong((elapsedTime + initialTime) * 2 / interval + 1f, 2f) - 1f;
        size.y = Mathf.Clamp(size.y * interval * 8f, 0f, 1f);
        transform.localScale = size;
    }
    void Update()
    {
        if (gameState.JudgeState("GamePlay"))
        {
            elapsedTime += Time.deltaTime;
            size.y = Mathf.PingPong((elapsedTime + initialTime) * 2 / interval + 1f, 2f) - 1f;
            size.y = Mathf.Clamp(size.y * interval * 8f, 0f, 1f);
            transform.localScale = size;
        }
    }
}
