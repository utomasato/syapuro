using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp_test : MonoBehaviour
{
    [SerializeField] GameObject fire;
    GameState_test state;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Gamestate = GameObject.Find("GameState");
        state = Gamestate.GetComponent<GameState_test>();
        Debug.Log(state);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Ignition()
    {
        //fire.GetComponent<MeshRenderer>().enabled = true;
        fire.GetComponent<SpriteRenderer>().enabled = true;
        state.AddScore(500);
    }
}
