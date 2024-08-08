using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampON : MonoBehaviour
{
    [SerializeField] private GameObject Lamp_Fire;//ランプにつく炎
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
        if (!Lamp_Fire.GetComponent<SpriteRenderer>().enabled)
        {
            Lamp_Fire.GetComponent<SpriteRenderer>().enabled = true;
            //   Lamp_Fire.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
