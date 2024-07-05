using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public GameObject fire;
    public Scores score;
    public Fire_test fire_test;
    public bool burning = false;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && burning == false && fire_test.GetIsOnCandle())
        {
            burning = true;
            fire.GetComponent<SpriteRenderer>().enabled = true;
            score.addscores(500);
        }
    }
}