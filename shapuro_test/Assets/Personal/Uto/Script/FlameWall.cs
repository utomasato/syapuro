using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameWall : MonoBehaviour
{
    private Vector3 size;
    private GameState gameState;
    bool isDying = false;
    [SerializeField] float time;
    private float elapsedTime;

    void Start()
    {
        size = new Vector3(1f, 1f, 1f);
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
    }

    void Update()
    {
        if (isDying)
        {
            if (gameState.JudgeState("GamePlay"))
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= time)
                {
                    elapsedTime = time;
                    isDying = false;
                }
                size.y = Mathf.Lerp(1f, 0f, elapsedTime / time);
                transform.localScale = size;
            }
        }
    }

    public void DyingOut()
    {
        isDying = true;
    }
}
