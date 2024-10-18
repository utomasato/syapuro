using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Fire fire_script;
    public Transform test;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Fire" && fire_script.GetIsOnCandle())
        {
            test.gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Fire" && fire_script.GetIsOnCandle())
        {
            test.gameObject.SetActive(true);
        }
    }
}
