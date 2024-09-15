using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class minicandle : MonoBehaviour
{
    [SerializeField] private GameObject fire;
    //[SerializeField] private GameState state;

    public void minicandlefire()
    {
        fire.GetComponent<SpriteRenderer>().enabled = true;
    }
}
