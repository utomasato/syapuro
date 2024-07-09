using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public GameObject fire;
    public Scores score;
    public Fire fire_script;
    public bool burning = false;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("Player");
        fire_script = Gamestate.GetComponent<Fire>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && burning == false && fire_script.GetIsCandle())
        {
            burning = true;
            fire.GetComponent<SpriteRenderer>().enabled = true;
            score.addscores(500);
        }
    }
}