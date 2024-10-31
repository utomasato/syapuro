using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lever : MonoBehaviour
{
    public float CreateTime;
    public float TransTime;
    public float TransTime_Dash;
    public Fire player;
    public GameState GameState;
    public Transform lever_B02;
    public enum Test
    {
        Mode_First,
        Mode_Second,
        Mode_TransF,
        Mode_TransS
    }
    public Test mode;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("Player");
        player = Gamestate.GetComponent<Fire>();
        GameObject Gamestate1 = GameObject.Find("GameState");
        GameState = Gamestate1.GetComponent<GameState>();
        lever_B02.transform.Rotate(0.0f, 0.0f, 45.0f);
        TransTime_Dash = 90 / (TransTime * 60);
    }
    void FixedUpdate()
    {
        if (GameState.JudgeState("Pause"))
        {

        }
        //Vector3 worldAngle = lever_B02.eulerAngles;
        else if (mode == Test.Mode_TransF || mode == Test.Mode_TransS)
        {
            //CreateTime += Time.deltaTime;
            //Debug.Log(lever_B02.transform.localEulerAngles.z);
            if (mode == Test.Mode_TransF/* && CreateTime > 0.1f*/)
            {
                lever_B02.transform.Rotate(0.0f, 0.0f, -TransTime_Dash);
                //CreateTime = 0.0f;
            }
            if (mode == Test.Mode_TransS/* && CreateTime > 0.1f */)
            {
                lever_B02.transform.Rotate(0.0f, 0.0f, TransTime_Dash);
                //CreateTime = 0.0f;
            }
            if (lever_B02.transform.localEulerAngles.z >= 315.0f && lever_B02.transform.localEulerAngles.z <= 316.0f && mode == Test.Mode_TransF)
            {
                mode = Test.Mode_Second;
            }
            if (lever_B02.transform.localEulerAngles.z >= 45.0f && lever_B02.transform.localEulerAngles.z <= 46.0f && mode == Test.Mode_TransS)
            {
                mode = Test.Mode_First;
            }
            // if (CreateTime > TransTime + 0.2f && mode == Test.Mode_TransF)
            // {
            //     Debug.Log("aaa");
            //     mode = Test.Mode_Second;
            //     //lever_B02.transform.Rotate(0.0f, 0.0f, -90.0f);
            //     worldAngle.z = 315.0f;
            //     lever_B02.eulerAngles = worldAngle;
            //     CreateTime = 0.0f;
            // }
            // if (CreateTime > TransTime + 0.2f && mode == Test.Mode_TransS)
            // {
            //     Debug.Log("bbb");
            //     mode = Test.Mode_First;
            //     //lever_B02.transform.Rotate(0.0f, 0.0f, 90.0f);
            //     worldAngle.z = 45.0f;
            //     lever_B02.eulerAngles = worldAngle;
            //     CreateTime = 0.0f;
            // }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && player.GetIsOnCandle())
        {
            if (mode == Test.Mode_First)
            {
                mode = Test.Mode_TransF;
            }
            if (mode == Test.Mode_Second)
            {
                mode = Test.Mode_TransS;
            }
        }
    }
}
