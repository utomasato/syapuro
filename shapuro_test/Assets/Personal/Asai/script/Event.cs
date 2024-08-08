using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    public Fire fire_script;
    public GameObject Event_img;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("Player");
        fire_script = Gamestate.GetComponent<Fire>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Fire")
        {
            Event_img.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Fire")
        {
            Event_img.SetActive(false);
        }
    }
}
