using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public GameState GameState;
    public Fire fire_script;
    public GameObject Door_hit;
    public GameObject KeyMake;
    public GameObject KeyImage;
    public GameObject Door01;
    public GameObject Door02;
    public float CountTime;
    public enum Test
    {
        Mode_Stay,
        Mode_First,
        Mode_First_2,
        Mode_First_3,
        Mode_First_4,
        Mode_Second,
        Mode_Finish,
    }
    public Test mode;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("Player");
        fire_script = Gamestate.GetComponent<Fire>();
        GameObject Gamestate2 = GameObject.Find("GameState");
        GameState = Gamestate2.GetComponent<GameState>();
    }
    void FixedUpdate()
    {
        if (!GameState.JudgeState("Pause"))
        {
            Vector3 poss = this.transform.position;
            poss.z = -5;
            KeyImage.transform.position = poss;
            if (mode == Test.Mode_First)
            {
                this.transform.position = KeyMake.transform.position;
                mode = Test.Mode_First_2;
            }
            if (mode == Test.Mode_First_2)
            {
                this.transform.Translate(Vector3.forward * -0.1f);
                CountTime += Time.deltaTime;
                if (CountTime > 0.5f)
                {
                    CountTime = 0f;
                    mode = Test.Mode_First_3;
                }
                //mode = Test.Mode_First_3;
            }
            if (mode == Test.Mode_First_3)
            {
                CountTime += Time.deltaTime;
                if (CountTime > 0.5f)
                {
                    CountTime = 0f;
                    mode = Test.Mode_First_4;
                }
                //mode = Test.Mode_First_4;
            }
            if (mode == Test.Mode_First_4)
            {
                Vector3 vector3 = Door_hit.transform.position - this.transform.position;
                //vector3.z = 0;
                Quaternion quaternion = Quaternion.LookRotation(vector3);
                this.transform.rotation = quaternion;
                this.transform.Translate(Vector3.forward * 0.1f);
            }
            if (mode == Test.Mode_Second)
            {
                Door01.gameObject.SetActive(false);
                Door02.gameObject.SetActive(true);
                Vector3 pos = this.transform.position;
                pos.x = -100;
                this.transform.position = pos;
                mode = Test.Mode_Finish;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        // if (other.gameObject.name == "Fire" && fire_script.GetIsOnCandle() && mode == Test.Mode_Stay)
        // {
        //     mode = Test.Mode_First;
        // }
        if (other.gameObject.name == "Door_hit" && mode == Test.Mode_First_4)
        {
            //Debug.Log("aaa");
            mode = Test.Mode_Second;
        }
    }
    public bool JudgeMode(string targetState)
    {
        return mode.ToString() == targetState;
    }
    public void TransMode(string targetState)
    {
        if (targetState == "Mode_Stay")
        {
            mode = Test.Mode_Stay;
        }
        if (targetState == "Mode_First")
        {
            mode = Test.Mode_First;
        }
        if (targetState == "Mode_Second")
        {
            mode = Test.Mode_Second;
        }
        if (targetState == "Mode_Finish")
        {
            mode = Test.Mode_Finish;
        }
    }
}
