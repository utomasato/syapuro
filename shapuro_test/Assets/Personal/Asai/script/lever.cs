using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lever : MonoBehaviour
{
    public float CreateTime;
    public float TransTime;
    public Fire player;
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
    }
    void FixedUpdate()
    {
        if (mode == Test.Mode_TransF || mode == Test.Mode_TransS)
        {
            CreateTime += Time.deltaTime;
            if (CreateTime > TransTime && mode == Test.Mode_TransF)
            {
                Debug.Log("aaa");
                mode = Test.Mode_Second;
                CreateTime = 0.0f;
            }
            if (CreateTime > TransTime && mode == Test.Mode_TransS)
            {
                Debug.Log("bbb");
                mode = Test.Mode_First;
                CreateTime = 0.0f;
            }
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
