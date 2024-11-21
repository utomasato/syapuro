using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycreate : MonoBehaviour
{
    public Fire fire_script;
    public RockedDoor RockedDoor;
    public float CreateTime;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("Player");
        fire_script = Gamestate.GetComponent<Fire>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && (Input.GetKey("left " + KeyBindings.DashKay) || Input.GetKey("right " + KeyBindings.DashKay) || Input.GetKey(KeyBindings.DashButton))/*Input.GetKey(KeyCode.Space)*/ && RockedDoor.Rock == false && fire_script.GetIsOnCandle())
        {
            Createkey(2);
        }
        else if (other.gameObject.name == "Fire" && RockedDoor.Rock == false && fire_script.GetIsOnCandle())
        {
            Createkey(1);
        }
    }
    void Createkey(int test)
    {
        if (test == 2)
        {
            CreateTime += Time.deltaTime * 10.0f;
        }
        else
        {
            CreateTime += Time.deltaTime;
        }
        if (CreateTime > 2f)
        {
            RockedDoor.Rock = true;
        }
    }
}
