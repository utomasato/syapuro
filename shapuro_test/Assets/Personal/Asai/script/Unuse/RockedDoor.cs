using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockedDoor : MonoBehaviour
{
    public GameObject lock_image;
    public Fire fire_script;
    public bool Rock = false;
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
        if (other.gameObject.name == "Fire" && fire_script.GetIsOnCandle() && Rock == true)
        {
            lock_image.GetComponent<SpriteRenderer>().enabled = false;
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