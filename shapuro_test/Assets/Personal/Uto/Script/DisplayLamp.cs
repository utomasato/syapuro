using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLamp : MonoBehaviour
{
    [SerializeField] private Lamp lamp;
    [SerializeField] private GameObject fire;
    [SerializeField] private GameState state;

    public void Ignition()
    {
        if (lamp != null)
        {
            if (lamp.GetBurning)
            {
                fire.GetComponent<Image>().enabled = true;
                state.gameLampSE();
            }
        }
        else
        {
            fire.GetComponent<Image>().enabled = true;
        }
    }

    public void Extinguishment()
    {
        fire.GetComponent<Image>().enabled = false;
    }
}
