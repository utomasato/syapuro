using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leverdoor : MonoBehaviour
{
    public GameState GameState;
    public lever lever;
    public enum Test
    {
        Mode_TransF,
        Mode_TransS
    }
    public Test mode;
    public float FirstScale;
    public float FirstPosition;
    public float Scale_dash;
    public GameObject Door_hit;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("GameState");
        GameState = Gamestate.GetComponent<GameState>();
        Vector3 s = transform.localScale;
        FirstScale = s.y;
        Scale_dash = s.y;
        Vector3 p = transform.position;
        FirstPosition = p.y;
        if (mode == Test.Mode_TransF && lever.mode == lever.Test.Mode_Second)
        {
            s.y = 0;
            p.y = 0.5f * FirstScale + FirstPosition;
            transform.localScale = s;
            transform.position = p;
            Door_hit.gameObject.SetActive(false);
            //Debug.Log("aho1");
        }
        if (mode == Test.Mode_TransS && lever.mode == lever.Test.Mode_First)
        {
            s.y = 0;
            p.y = 0.5f * FirstScale + FirstPosition;
            transform.localScale = s;
            transform.position = p;
            Door_hit.gameObject.SetActive(false);
            //Debug.Log("aho2");
        }
        FirstPosition = FirstScale / (lever.TransTime * 60) * 0.5f;
        FirstScale = FirstScale / (lever.TransTime * 60);
    }
    void FixedUpdate()
    {
        Vector3 s = transform.localScale;
        Vector3 p = transform.position;
        if (s.y <= 0.0f || GameState.JudgeState("Pause"))
        {

        }
        else if (mode == Test.Mode_TransF && lever.mode == lever.Test.Mode_TransF)
        {
            // s.y = 0;
            // p.y = 0.5f * FirstScale + FirstPosition;
            s.y -= FirstScale;
            p.y += FirstPosition;
            transform.localScale = s;
            transform.position = p;
            Door_hit.gameObject.SetActive(false);
            // Debug.Log(FirstScale);
            // Debug.Log(FirstPosition);
        }
        if (s.y <= 0.0f || GameState.JudgeState("Pause"))
        {

        }
        else if (mode == Test.Mode_TransS && lever.mode == lever.Test.Mode_TransS)
        {
            // s.y = 0;
            // p.y = 0.5f * FirstScale + FirstPosition;
            s.y -= FirstScale;
            p.y += FirstPosition;
            transform.localScale = s;
            transform.position = p;
            Door_hit.gameObject.SetActive(false);
            //Debug.Log("aa");
        }
        if (s.y >= Scale_dash || GameState.JudgeState("Pause"))
        {

        }
        else if (mode == Test.Mode_TransF && lever.mode == lever.Test.Mode_TransS)
        {
            // s.y = FirstScale;
            // p.y = FirstPosition;
            s.y += FirstScale;
            p.y -= FirstPosition;
            transform.localScale = s;
            transform.position = p;
            Door_hit.gameObject.SetActive(true);
        }
        if (s.y >= Scale_dash || GameState.JudgeState("Pause"))
        {

        }
        else if (mode == Test.Mode_TransS && lever.mode == lever.Test.Mode_TransF)
        {
            // s.y = FirstScale;
            // p.y = FirstPosition;
            s.y += FirstScale;
            p.y -= FirstPosition;
            transform.localScale = s;
            transform.position = p;
            Door_hit.gameObject.SetActive(true);
        }
    }
}
