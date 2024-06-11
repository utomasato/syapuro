using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp_test : MonoBehaviour
{
    [SerializeField] GameObject fire;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Ignition()
    {
        //fire.GetComponent<MeshRenderer>().enabled = true;
        fire.GetComponent<SpriteRenderer>().enabled = true;
    }
}
