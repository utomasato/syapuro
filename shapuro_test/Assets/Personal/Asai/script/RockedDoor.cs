using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockedDoor : MonoBehaviour
{
    public Fire fire_script;
    public float Asai_testx = 1f;
    public bool Rock = false;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("Player");
        fire_script = Gamestate.GetComponent<Fire>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && Rock == true && fire_script.GetIsCandle())
        {
            transform.position += new Vector3(0, Asai_testx * Time.deltaTime, 0);
        }
    }
}