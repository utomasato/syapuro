using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Fire fire_script;
    public Transform test;
    public GameObject Door01;
    public GameObject Door02;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("Player");
        fire_script = Gamestate.GetComponent<Fire>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Fire" && fire_script.GetIsOnCandle())
        {
            test.gameObject.SetActive(false);
            Door01.gameObject.SetActive(false);
            Door02.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Fire" && fire_script.GetIsOnCandle())
        {
            test.gameObject.SetActive(true);
            Door01.gameObject.SetActive(true);
            Door02.gameObject.SetActive(false);
        }
    }
}
