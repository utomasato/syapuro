using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetDisplay : MonoBehaviour
{
    public bool used = false;
    public Fire fire_script;
    public GameState GameState;
    public Text Text;
    public Color spriteColor;
    public float Times = 0.0f;
    public float duration = 1f;
    void Start()
    {
        GameObject Gamestate1 = GameObject.Find("Player");
        fire_script = Gamestate1.GetComponent<Fire>();
        GameObject Gamestate2 = GameObject.Find("GameState");
        GameState = Gamestate2.GetComponent<GameState>();
        //sp = GetComponent();
        spriteColor = Text.color;
        //StartCoroutine(Fade(0));
    }
    void FixedUpdate()
    {
        if (used == false && GameState.JudgeState("GamePlay"))
        {
            //Event_img.SetActive(true);
            StartCoroutine(Fade(1));
            used = true;
        }
        if (spriteColor.a == 1 && Times > 0.5f)
        {
            //Event_img.SetActive(false);
            StartCoroutine(Fade(0));
        }
        else if (spriteColor.a == 1 && used == true)
        {
            Times += Time.deltaTime;
        }
    }
    IEnumerator Fade(float targetAlpha)
    {
        while (!Mathf.Approximately(spriteColor.a, targetAlpha))
        {
            float changePerFrame = Time.deltaTime / duration;
            spriteColor.a = Mathf.MoveTowards(spriteColor.a, targetAlpha, changePerFrame);
            Text.color = spriteColor;
            yield return null;
        }
    }
}