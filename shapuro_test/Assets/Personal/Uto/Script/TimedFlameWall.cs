using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedFlameWall : MonoBehaviour
{
    [SerializeField] private float interval = 10f;
    private float elapsedTime;
    [SerializeField] private float initialTime;
    private Vector3 size;
    private GameState gameState;
    [SerializeField] Image BurningGauge, SilentGauge;

    [SerializeField] bool windMode;
    [SerializeField] ParticleSystem windParticle;
    [SerializeField] float maxRange;
    [SerializeField] float particleMargin;
    ParticleSystem.MainModule main;

    void Start()
    {
        elapsedTime = 0f;
        size = new Vector3(1f, 1f, 1f);
        gameState = GameObject.Find("GameState").GetComponent<GameState>();

        if (BurningGauge != null)
            if (size.y > 0f)
            {
                BurningGauge.fillAmount = 1f - (elapsedTime + initialTime) / interval % 2f;
                SilentGauge.fillAmount = 0f;
            }
            else
            {
                BurningGauge.fillAmount = 1f;
                SilentGauge.fillAmount = 2f - (elapsedTime + initialTime) / interval % 2f;
            }

        size.y = Mathf.PingPong((elapsedTime + initialTime) * 2 / interval + 1f, 2f) - 1f;
        size.y = Mathf.Clamp(size.y * interval * 8f, 0f, 1f);
        transform.localScale = size;

        if (windMode)
        {
            main = windParticle.main;
            main.startLifetime = (maxRange * size.y + particleMargin) / 100;
        }
    }
    void Update()
    {
        if (gameState.JudgeState("GamePlay"))
        {
            elapsedTime += Time.deltaTime;
            size.y = Mathf.PingPong((elapsedTime + initialTime) * 2 / interval + 1f, 2f) - 1f;
            if (BurningGauge != null)
                if (size.y > 0f)
                {
                    //Debug.Log(1f - (elapsedTime + initialTime) / interval % 2f);
                    BurningGauge.fillAmount = 1f - (elapsedTime + initialTime) / interval % 2f;
                    SilentGauge.fillAmount = 0f;
                }
                else
                {
                    BurningGauge.fillAmount = 1f;
                    //Debug.Log(2f - (elapsedTime + initialTime) / interval % 2f);
                    SilentGauge.fillAmount = 2f - (elapsedTime + initialTime) / interval % 2f;
                }
            size.y = Mathf.Clamp(size.y * interval * 8f, 0f, 1f);
            transform.localScale = size;

            if (windMode)
            {
                main.startLifetime = (maxRange * size.y + particleMargin) / 100;
            }
        }
    }
}
