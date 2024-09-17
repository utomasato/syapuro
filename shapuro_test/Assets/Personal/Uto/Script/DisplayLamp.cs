using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLamp : MonoBehaviour
{
    //[SerializeField] private Lamp lamp;
    [SerializeField] private GameObject fire;
    //[SerializeField] private GameState state;

    public void Ignition(GameState state = null)
    {
        fire.GetComponent<Image>().enabled = true;
        if (state != null)
            state.gameLampSE();
    }

    public void Extinguishment()
    {
        fire.GetComponent<Image>().enabled = false;
    }
}
